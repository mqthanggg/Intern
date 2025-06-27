using Swashbuckle.AspNetCore.Annotations;

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
}