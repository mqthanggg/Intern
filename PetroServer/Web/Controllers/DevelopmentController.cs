public static class DevelopmentController{
    public static WebApplication MapSignup(this WebApplication app){
        app.MapPut("/signup", SignupAccount);
        return app;
    }
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    [SwaggerOperation(
        Summary = "Signup a development account.",
        Description = @"Signup development accounts.
        - User: 
        + mqthanggg, admin123
        + linh, admin123
        - Administrator:
        + admin, admin123"
    )]
    public static async Task<IResult> SignupAccount(IHasher hasher, IUserRepository userRepository){
        while (true){
            try{
                (string hashedPassword, string padding) = hasher.Hash(new object{},"admin123");
                _ = await userRepository.InsertAsync(new User{
                    Username = "mqthanggg",
                    Password = hashedPassword,
                    Role = 1,
                    Padding = padding,
                    CreatedBy = "server",
                    LastModifiedBy = "server"
                });
                (hashedPassword, padding) = hasher.Hash(new object{},"admin123");
                _ = await userRepository.InsertAsync(new User{
                    Username = "linh",
                    Password = hashedPassword,
                    Role = 1,
                    Padding = padding,
                    CreatedBy = "server",
                    LastModifiedBy = "server"
                });
                (hashedPassword, padding) = hasher.Hash(new object{},"admin123");
                _ = await userRepository.InsertAsync(new User{
                    Username = "admin",
                    Password = hashedPassword,
                    Role = 2,
                    Padding = padding,
                    CreatedBy = "server",
                    LastModifiedBy = "server"
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
                    return TypedResults.InternalServerError(new ErrorResponse{Why = column});
                }
                return TypedResults.InternalServerError(new ErrorResponse{Why = e.Message});
            }
            break;
        }
        return TypedResults.Ok();
    }
}