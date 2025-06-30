Env.Load();

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.JSONSetup();
builder.Services.AuthSetup(env);
builder.Services.AddEndpointsApiExplorer();
builder.Services.HealthCheckSetup();
builder.Services.SwaggerSetup();
builder.Services.DbSetup();
builder.Services.AddServices();
builder.Logging.AddSimpleConsole(c => c.SingleLine = true);

DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
var report = await healthCheckService.CheckHealthAsync();

if (report.Status == HealthStatus.Healthy){
    app.UseRouting();
    app.UseCors();
    if (app.Environment.IsDevelopment()){
        IdentityModelEventSource.ShowPII = true; 
        app.Swagger();
        app.MapSignup();
    }
    app.UseMiddlewares();
    app.MapPublicController();
    // app.MapGet("log/station/{id}", [Authorize] async([FromRoute] int id) => {
    //     string schemaName = Env.GetString("SCHEMA");
    //     await using var cmd = db_read_dataSource.CreateCommand($@"
    //     SELECT dp.name, log.fuel_name, log.total_liters, fuel.price, log.total_amount, log.time FROM
    //     {schemaName}.log as log
    //     INNER JOIN {schemaName}.dispenser as dp ON dp.dispenser_id = log.dispenser_id AND dp.station_id = @stationId
    //     INNER JOIN {schemaName}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
    //     ");
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "stationId", Value = id});
    //     var res = await cmd.ExecuteReaderAsync();
    //     var dataTable = new DataTable();
    //     dataTable.Load(res);
    //     await res.CloseAsync();
    //     return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    // });
    // app.MapGet("/dispenser/station/{id}", [Authorize]  async ([FromRoute] int id) => {
    //     string schemaName = Env.GetString("SCHEMA");
    //     await using var cmd = db_read_dataSource.CreateCommand($@"
        // SELECT dp.name, f.price, f.long_name, f.short_name FROM
        // {schemaName}.dispenser as dp
        // INNER JOIN {schemaName}.fuel as f ON f.fuel_id = dp.fuel_id AND dp.station_id = @id
    //     ");
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "id", Value = id});
    //     var res = await cmd.ExecuteReaderAsync();
    //     var dataTable = new DataTable();
    //     dataTable.Load(res);
    //     await res.CloseAsync();
    //     return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    // });
    // app.MapGet("/tank/station/{id}", [Authorize]  async ([FromRoute] int id) => {
    //     string schemaName = Env.GetString("SCHEMA");
    //     await using var cmd = db_read_dataSource.CreateCommand($@"
    //     SELECT t.name, f.short_name, t.max_volume FROM
    //     {schemaName}.tank as t
    //     INNER JOIN {schemaName}.fuel as f ON f.fuel_id = t.fuel_id AND t.station_id = @id
    //     ");
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "id", Value = id});
    //     var res = await cmd.ExecuteReaderAsync();
    //     var dataTable = new DataTable();
    //     dataTable.Load(res);
    //     await res.CloseAsync();
    //     return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    // });
    // app.MapDelete("/station/{id}", [Authorize] async (int id) => {
    //     await using var cmd = db_write_dataSource.CreateCommand($@"
    //     DELETE FROM {Env.GetString("SCHEMA")}.station 
    //     WHERE station_id = @stationId
    //     ");
    //     cmd.Parameters.Clear();
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "stationId", Value = id});
    //     var res = await cmd.ExecuteNonQueryAsync();
    //     if (res > 0) 
    //         return Results.Ok();
    //     return Results.BadRequest();
    // });
    // app.MapPut("/station/{id}", [Authorize] async (int id, Station body) => {
    //     await using var cmd = db_write_dataSource.CreateCommand($@"
    //     UPDATE {Env.GetString("SCHEMA")}.station
    //     SET 
    //     name = @name,
    //     address = @address
    //     WHERE
    //     station_id = @stationId
    //     ");
    //     cmd.Parameters.Clear();
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "stationId", Value = id});
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "name", Value = body.Name});
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "address", Value = body.Address});
    //     var res = await cmd.ExecuteNonQueryAsync();
    //     if (res > 0)
    //         return Results.Ok();
    //     return Results.BadRequest();
    // });
    // app.MapGet("/stations", [Authorize] async () => {
    //     await using var cmd = db_read_dataSource.CreateCommand($"SELECT station_id,name,address FROM {Env.GetString("schema")}.station");
    //     var res = await cmd.ExecuteReaderAsync();
    //     var dataTable = new DataTable();
    //     dataTable.Load(res);
    //     await res.CloseAsync();
    //     return JsonConvert.SerializeObject(dataTable,Formatting.Indented);
    // });
    // app.MapPost("/login", async ([FromBody] LoginRequest body) => {
    //     await using var cmd = db_read_dataSource.CreateCommand($"SELECT user_id,username,password,padding FROM {Env.GetString("schema")}.user WHERE username = @username");
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "username", Value = body.Username});
    //     var res = await cmd.ExecuteReaderAsync();
    //     if (!res.HasRows){
    //         await res.CloseAsync();
    //         return Results.Unauthorized();
    //     }
    //     res.Read();
    //     var encryptedPassword = res["password"].ToString() ?? "";
    //     var passwordPadding = res["padding"].ToString() ?? "";
    //     var userId = Convert.ToInt32(res["user_pasid"]);
    //     if (PasswordHasher.Verify(body,body.Password + passwordPadding,encryptedPassword)){
    //         var claims = new List<Claim>{
    //             new Claim(ClaimTypes.Sid, userId.ToString()),
    //             new Claim(ClaimTypes.Name, res["username"].ToString() ?? "")
    //         };
    //         var creds = new SigningCredentials(new RsaSecurityKey(RSAClass.LoadPrivateRsaKey()),SecurityAlgorithms.RsaSha256);
    //         var tokenDescriptor = new JwtSecurityToken(
    //             issuer: Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
    //             audience: Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
    //             claims: claims,
    //             expires: DateTime.UtcNow.AddMinutes(5),
    //             signingCredentials: creds
    //         );
    //         using var refreshTokenCmd = db_write_dataSource.CreateCommand($@"
    //             UPDATE {Env.GetString("SCHEMA")}.user
    //             SET 
    //             refresh_token = @refreshToken,
    //             token_padding = @refreshTokenPadding,
    //             token_expired_time = now() + INTERVAL '7 days'
    //             WHERE
    //             user_id = @userId
    //         ");
    //         string randomRefreshToken;
    //         using (var _rng = RandomNumberGenerator.Create()){
    //             byte[] randomBytes = new byte[16];
    //             _rng.GetBytes(randomBytes);
    //             randomRefreshToken = Convert.ToBase64String(randomBytes);
    //         }
    //         res.Close();
    //         int col;
    //         while (true){
    //             List<string> hash = PasswordHasher.Hash(new {},randomRefreshToken);
    //             string hashedRefreshToken = hash[0];
    //             string refreshTokenPadding = hash[1];
    //             refreshTokenCmd.Parameters.Clear();
    //             refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "refreshToken", Value = hashedRefreshToken});
    //             refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "refreshTokenPadding", Value = refreshTokenPadding});
    //             refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "userId", Value = userId});
    //             try{
    //                 col = await refreshTokenCmd.ExecuteNonQueryAsync();
    //             }
    //             catch (PostgresException e){
    //                 if (e.SqlState == "23505"){
    //                     /*
    //                         Example constraint name: user_<column>_key
    //                     */
    //                     Regex regex = new Regex(@"user_(\w+)_key");
    //                     Match match = regex.Match(e.ConstraintName ?? "");
    //                     string column = match.Groups[1].Value;
    //                     if (column == "padding")//Rare
    //                         continue;
    //                     return Results.Json(new {why = column},statusCode: 400);
    //                 }
    //                 return Results.Json(new {why = e.Message}, statusCode:400);
    //             }
    //             break;
    //         }
    //         if (col > 0){
    //             return Results.Ok(new {
    //                 token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
    //                 refresh_token = randomRefreshToken
    //             });
    //         }
    //     }
    //     return Results.Unauthorized();
    // });
    // app.MapPost("/refresh", async ([FromHeader(Name = "Authorization")] string authHeader, TokenRequest body) => {
    //     var claims = (List<Claim>)new JwtSecurityTokenHandler().ReadJwtToken(authHeader.Substring(7)).Claims;
    //     var userId = claims.First(e => e.Type == ClaimTypes.Sid).Value;
    //     var username = claims.First(e => e.Type == ClaimTypes.Name).Value;
    //     await using var cmd = db_read_dataSource.CreateCommand($@"
    //         SELECT username, refresh_token, token_padding, token_expired_time FROM {Env.GetString("SCHEMA")}.user
    //         WHERE user_id = @userId
    //     ");
    //     cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "userId", Value = Convert.ToInt32(userId)});
    //     var res = await cmd.ExecuteReaderAsync();
    //     res.Read();
    //     if (!(res.HasRows && res["username"].ToString() == username))
    //         return Results.Unauthorized();
    //     var refreshTokenExpiredTime = Convert.ToDateTime(res["token_expired_time"]);
    //     var hashedRefreshToken = res["refresh_token"].ToString() ?? "";
    //     var refreshTokenPadding = res["token_padding"].ToString();
    //     if (
    //         DateTime.Now.CompareTo(refreshTokenExpiredTime) > 0 || //Expired
    //         !PasswordHasher.Verify(body, body.RefreshToken + refreshTokenPadding, hashedRefreshToken) //Invalid refresh token
    //     ) 
    //         return Results.Unauthorized();
    //     await res.CloseAsync();
    //     var creds = new SigningCredentials(new RsaSecurityKey(RSAClass.LoadPrivateRsaKey()),SecurityAlgorithms.RsaSha256);
    //     var tokenDescriptor = new JwtSecurityToken(
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddMinutes(5),
    //         signingCredentials: creds
    //     );
    //     return Results.Json(new {token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)}, statusCode: 200);
    // });
    // //Configure route for JWKS
    // app.MapGet("/.well-known/jwks.json",JWKsService.GetJWKs);
    app.Run();
}
else{
    await app.StopAsync();
}