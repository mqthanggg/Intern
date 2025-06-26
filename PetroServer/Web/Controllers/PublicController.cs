public static class PublicController{
    public static WebApplication MapPublicController(this WebApplication app){
        app.MapGet("log/station/{id}",GetLogByStationId);
        return app;
    }
    [Authorize]
    public static async Task<IResult> GetLogByStationId([FromRoute] int id, IDbQueryService dbQuery){
        var res = await dbQuery.ExecuteQueryAsync<Station,Log>(new Station{StationId = id},DbOperation.SELECT);
        return TypedResults.Ok(
            JsonConvert.SerializeObject(res, Formatting.Indented)
        );
    }
}