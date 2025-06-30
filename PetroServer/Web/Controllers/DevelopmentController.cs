public static class DevelopmentController{
    public static WebApplication MapSignup(this WebApplication app){
        app.MapPut("/signup", SignupAccount);
        return app;
    }
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Signup a development account.",
        Description = "Signup a development account with username, password are set to mqthanggg, admin123 (for development only)."
    )]
    public static async Task<IResult> SignupAccount(IHasher hasher, IUserRepository userRepository){
        while (true){
            (string hashedPassword, string padding) = hasher.Hash(new object{},"admin123");
            int affectedRows;
            try{
                affectedRows = await userRepository.InsertAsync(new User{
                Username = "mqthanggg",
                Password = hashedPassword,
                Padding = padding,
                CreatedBy = "mqthanggg",
                LastModifiedBy = "mqthanggg"
            });
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