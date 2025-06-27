using Microsoft.AspNetCore.Http.HttpResults;

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