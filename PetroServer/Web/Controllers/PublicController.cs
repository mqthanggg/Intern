public static class PublicController{
    public static WebApplication MapPublicController(this WebApplication app){
        app.MapGet("log/station/{id}",GetLogByStationId);
        app.MapGet("dispenser/station/{id}",GetDispenserByStationId);
        app.MapGet("tank/station/{id}",GetTankByStationId);
        app.MapGet(".well-known/jwks.json",GetJWKs);
        app.MapPost("login",Login);
        app.MapDelete("station/{id}",DeleteStationFromId);
        app.MapPut("station/{id}",UpdateStationFromId);
        app.MapGet("stations",GetStations);
        app.MapPost("refresh", RefreshJWT);
        return app;
    }
    [Authorize]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetLogByStationId([FromRoute] int id, ILogRepository logRepository)
    {
        var res = await logRepository.GetLogByStationIdAsync(new Station{StationId = id});
        return TypedResults.Ok(
            res
        );
    }
    [Authorize]
    [ProducesResponseType(typeof(List<DispenserResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispensers by station ID.",
        Description = "Retrieve a list of dispensers that belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId([FromRoute] int id, IDispenserRepository dispenserRepository)
    {
        var res = await dispenserRepository.GetDispensersByStationIdAsync(new Station{StationId = id});
        return TypedResults.Ok(
            res
        );
    }
    [Authorize]
    [ProducesResponseType(typeof(List<TankResponse>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve tanks by station ID.",
        Description = "Retrieve a list of tanks that are belong to the given station."
    )]
    public static async Task<IResult> GetTankByStationId([FromRoute] int id, ITankRepository tankRepository){
        var res = await tankRepository.GetTanksByStationIdAsync(new Station{StationId = id});
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
    [ProducesResponseType(typeof(LoginResponse),200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Login",
        Description = "Resolve login request from the client."
    )]
    public static async Task<IResult> Login([FromBody] LoginRequest body, IUserRepository userRepository, IHasher hasher, IJWTService jwt)
    {
        try
        {
            var user = await userRepository.GetUserByUsernameAsync(new User{Username = body.Username});
            if (hasher.Verify(user, body.Password + user.Padding, user.Password!))
            {
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
                        res = await userRepository.UpdateAsync(new User { 
                                UserId = user.UserId,
                                RefreshToken = hashed,
                                TokenPadding = padding,
                                LastModifiedBy = body.Username,
                            });
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
                            return TypedResults.InternalServerError(new ErrorResponse{Why = column});
                        }
                        return TypedResults.InternalServerError(new ErrorResponse{Why = e.Message});
                    }
                    break;
                }
                if (res > 0)
                {
                    return Results.Ok(new
                    LoginResponse{
                        Token = jwt.GenerateAccessToken(user.UserId!.Value, user.Username!),
                        RefreshToken = randomRefreshToken
                    });
                }
            }
            return TypedResults.Unauthorized();
        }
        catch (InvalidOperationException)
        {
            return TypedResults.NotFound();
        }
    }
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Delete station",
        Description = "Delete a station from a given id."
    )]
    public static async Task<IResult> DeleteStationFromId([FromRoute] int id, IStationRepository stationRepository){
        int res;
        try
        {
            res = await stationRepository.DeleteAsync(new Station{StationId = id});
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
        if (res == 1)
            return TypedResults.Ok();
        return TypedResults.NotFound();
    }
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Update station",
        Description = "Update name, address of a station from a given id."
    )]
    public static async Task<IResult> UpdateStationFromId([FromRoute] int id, [FromBody] StationUpdateRequest body, HttpContext httpContext, IStationRepository stationRepository, IJWTService jWTService){
        string bearer = httpContext.Request.Headers.Authorization.FirstOrDefault() ?? "";
        string username = jWTService.GetClaims(bearer).First(e => e.Type == ClaimTypes.Name).Value;
        int res;
        try
        {
            res = await stationRepository.UpdateAsync(new Station{StationId = id, Name = body.Name, Address = body.Address, LastModifiedBy = username});
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
        if (res == 1)
            return TypedResults.Ok();
        return TypedResults.NotFound();
    }
    [Authorize]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(List<StationResponse>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain stations.",
        Description = "Obtain a list of all stations."
    )]
    public static async Task<IResult> GetStations(IStationRepository stationRepository){
        try
        {
            var res = await stationRepository.GetAllStationResponseAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
            return TypedResults.InternalServerError();
        }
    }
    [ProducesResponseType(typeof(TokenResponse),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Refresh access token.",
        Description = "Return a new access token if the refresh token is valid and not expired to the user."
    )]
    public static async Task<IResult> RefreshJWT(ILogger<object> logger, HttpContext httpContext, [FromBody] TokenRequest tokenRequest, IUserRepository userRepository, IJWTService jWTService, IHasher hasher){
        try
        {
            IReadOnlyList<Claim> claims = jWTService.GetClaims(httpContext.Request.Headers.Authorization.First() ?? "");
            int userId = Convert.ToInt32(claims.First(e => e.Type == ClaimTypes.Sid).Value);
            string username = claims.First(e => e.Type == ClaimTypes.Name).Value;
            var user = await userRepository.GetUserTokenAsync(new User{
                UserId = userId,
                Username = username
            });
            if (
                DateTime.UtcNow.CompareTo(user.TokenExpiredTime) > 0 ||
                !hasher.Verify(user, tokenRequest.RefreshToken + user.TokenPadding!, user.RefreshToken!)
            )
                return TypedResults.Unauthorized();
            return TypedResults.Ok(
                new TokenResponse{
                    Token = jWTService.GenerateAccessToken(
                        userId,
                        username
                    )
                }
            );
        }
        catch (Exception ex)
        {
            if (ex is InvalidDataException){
                return TypedResults.InternalServerError();
            }
            if (ex is PostgresException){
                return TypedResults.Unauthorized();
            }
        }

        return TypedResults.Ok();
    }
}