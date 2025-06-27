<<<<<<< HEAD
using Microsoft.AspNetCore.Http.HttpResults;
=======
using Swashbuckle.AspNetCore.Annotations;
>>>>>>> 3dee3400e5456b156d5cde66b101730f2b0b06f4

public static class PublicController{
    public static WebApplication MapPublicController(this WebApplication app){
        app.MapGet("log/station/{id}",GetLogByStationId);
        app.MapGet("dispenser/station/{id}",GetDispenserByStationId);
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
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId([FromRoute] int id, IDbQueryService dbQuery){
        var res = await dbQuery.ExecuteQueryAsync<Station,Dispenser>(new Station{StationId = id},DbOperation.SELECT);
        return TypedResults.Ok(
            res
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