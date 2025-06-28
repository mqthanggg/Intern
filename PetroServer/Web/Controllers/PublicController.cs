using Swashbuckle.AspNetCore.Annotations;

public static class PublicController{
    public static WebApplication MapPublicController(this WebApplication app){
       app.MapGet("log/station/{id}", GetLogByStationId);
        app.MapGet("dispenser/station/{id}", GetDispenserByStationId);
        app.MapPost("login", Login);
        return app;
    }
    // [Authorize]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetLogByStationId([FromRoute] int id, IDbQueryService dbQuery)
    {
        var res = await dbQuery.ExecuteQueryAsync<Station, Log>(new Station { StationId = id }, DbOperation.SELECT);
        return TypedResults.Ok(
            res
        );
    }
    // [Authorize]
    [ProducesResponseType(typeof(List<DispenserResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispensers by station ID.",
        Description = "Retrieve a list of dispensers that belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId([FromRoute] int id, IDbQueryService dbQuery)
    {
        var res = await dbQuery.ExecuteQueryAsync<Station, Dispenser>(new Station { StationId = id }, DbOperation.SELECT);
        return TypedResults.Ok(
            res
        );
    }
    // [Authorize]
    [ProducesResponseType(typeof(List<TankResponse>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve tanks by station ID.",
        Description = "Retrieve a list of tanks that are belong to the given station."
    )]
    public static async Task<IResult> GetTankByStationId([FromRoute] int id, IDbQueryService dbQuery){
        var res = await dbQuery.ExecuteQueryAsync<Station,Tank>(new Station{StationId = id},DbOperation.SELECT);
        return TypedResults.Ok(
            res
        );
    }

    [ProducesResponseType(typeof(List<JsonWebKey>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve JWKs",
        Description = "Retrieve a list of JSON web key."
    )]
    public static IResult GetJWKs(IJWKsService jWKs){
        return TypedResults.Ok(
            jWKs.GetJWKs()
        );
    }
    public static async Task<IResult> Login([FromBody] LoginRequest body, IDbQueryService dbQuery, IHasher hasher, IJWTService ijwt)
    {
        try
        {
            var result = await dbQuery.ExecuteQueryAsync<User, User>(
                new User { Username = body.Username },
                DbOperation.SELECT
            );
            var user1 = result as User;
            if (user1 == null)
                return TypedResults.NotFound("Tài khoản không tồn tại");
            if (hasher.Verify(result, body.Password + user1.Padding, user1.Password))
            {
                return TypedResults.Ok(new
                {
                    token = ijwt.GenerateAccessToken(user1.UserId, user1.Username)
                });
            }
              string randomRefreshToken;
            using (var _rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[16];
                _rng.GetBytes(randomBytes);
                randomRefreshToken = Convert.ToBase64String(randomBytes);
            }
            int res;
            while (true)
            {
                (string hashed, string padding) = hasher.Hash(new { }, randomRefreshToken);
                try
                {
                    res = (int)await dbQuery.ExecuteQueryAsync<User, User>(new User { Username = body.Username }, DbOperation.UPDATE);
                }
                catch (PostgresException e)
                {
                    if (e.SqlState == "23505")
                    {
                        /*
                            Example constraint name: user_<column>_key
                        */
                        Regex regex = new Regex(@"user_(\w+)_key");
                        Match match = regex.Match(e.ConstraintName ?? "");
                        string column = match.Groups[1].Value;
                        if (column == "token_padding")//Rare
                            continue;
                        return TypedResults.InternalServerError(new { why = column });
                    }
                    return TypedResults.InternalServerError(new { why = e.Message });
                }
                break;
            }
            if (res > 0)
            {
                return Results.Ok(new
                {
                    token = ijwt.GenerateAccessToken(user1.UserId, user1.Username),
                    refresh_token = randomRefreshToken
                });
            }
            return TypedResults.NotFound();
        }
        catch (InvalidOperationException)
        {
            return TypedResults.NotFound();
        }
    }

}
