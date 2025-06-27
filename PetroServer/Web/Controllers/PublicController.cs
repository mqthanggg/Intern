using Swashbuckle.AspNetCore.Annotations;

public static class PublicController{
    public static WebApplication MapPublicController(this WebApplication app){
        app.MapGet("log/station/{id}",GetLogByStationId);
        app.MapGet("dispenser/station/{id}",GetDispenserByStationId);
        app.MapGet("tank/station/{id}",GetTankByStationId);
        app.MapGet("/.well-known/jwks.json",GetJWKs);
        return app;
    }
    // [Authorize]
    [ProducesResponseType(typeof(List<LogResponse>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetLogByStationId([FromRoute] int id, IDbQueryService dbQuery){
        var res = await dbQuery.ExecuteQueryAsync<Station,Log>(new Station{StationId = id},DbOperation.SELECT);
        return TypedResults.Ok(
            res
        );
    }
    // [Authorize]
    [ProducesResponseType(typeof(List<DispenserResponse>),200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispensers by station ID.",
        Description = "Retrieve a list of dispensers that belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId([FromRoute] int id, IDbQueryService dbQuery){
        var res = await dbQuery.ExecuteQueryAsync<Station,Dispenser>(new Station{StationId = id},DbOperation.SELECT);
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
        User obj;
        try
        {
            obj = (User)await dbQuery.ExecuteQueryAsync<User, User>(new User { Username = body.Username }, DbOperation.SELECT);
        }
        catch (InvalidOperationException)
        {
            return TypedResults.NotFound();
        }
        if(hasher.Verify(body, body.Password + obj.Padding, obj.Password))
        {
            return TypedResults.Ok(new
            {
                token = ijwt.GenerateAccessToken(obj.UserId, obj.Username)
            });

        }  return TypedResults.NotFound(new { message = "Password not true" });
    }
}