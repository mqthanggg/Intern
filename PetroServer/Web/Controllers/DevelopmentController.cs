public static class DevelopmentController{
    public static async Task<IResult> SignupAccount(IDbWrite dbWrite, IHasher hasher, IAuditService audit){
        var dbWriteDataSource = dbWrite.DataSource;
        await using var cmd = dbWriteDataSource.CreateCommand($@"
            INSERT INTO {Env.GetString("SCHEMA")}.user(username, password, padding) VALUES (@username, @password, @padding)
        ");
        while (true){
            (string hashedPassword, string padding) = hasher.Hash(new object{},"admin123");
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "username", Value = "mqthanggg"});
            cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "password", Value = hashedPassword});
            cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "padding", Value = padding});
            int res;
            try{
                res = await audit.ExecuteWriteQueryWithUpdateAuditAsync(cmd, "user", DbOperation.INSERT);
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