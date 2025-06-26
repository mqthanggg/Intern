public static class DevelopmentController{
    public static WebApplication MapSignup(this WebApplication app){
        app.MapPut("/signup", SignupAccount);
        return app;
    }
    public static async Task<IResult> SignupAccount(IHasher hasher, IDbQueryService dbQuery){
        while (true){
            (string hashedPassword, string padding) = hasher.Hash(new object{},"admin123");
            var user = new User{
                Username = "mqthanggg",
                Password = hashedPassword,
                Padding = padding,
            }; 
            int affectedRows;
            try{
                affectedRows = (int)await dbQuery.ExecuteQueryAsync<User,User>(user, DbOperation.INSERT);
            }
            catch (PostgresException e){
                if (e.SqlState == "23505"){
                    /*
                        Example constraint name: user_<column>_key
                    */
                    Regex regex = new Regex(@"user_(\w+)_key");
                    Match match = regex.Match(e.ConstraintName ?? "");
                    string column = match.Groups[1].Value;
                    if (column == "padding")//Rare
                        continue;
                    return TypedResults.InternalServerError(new {why = column});
                }
                return TypedResults.InternalServerError(new {why = e.Message});
            }
            break;
        }
        return TypedResults.Ok();
    }
}