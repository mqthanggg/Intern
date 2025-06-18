using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Npgsql;


DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(c => c.SingleLine = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks().AddNpgSql(DotNetEnv.Env.GetString("DBWRITE_CONNECTION_STRING"),name:"write").AddNpgSql(DotNetEnv.Env.GetString("DBREAD_CONNECTION_STRING"),name:"read");
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1",new OpenApiInfo{Title = "Petro application API", Description = "List of APIs for petro application", Version = "v1"});
    c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{
        Description = "JWT Authorization using Bearer header",
        Name = "JWT Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "JWT Authorization",
                In = ParameterLocation.Header,},
            new List<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op => {
    op.Authority = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY");
    op.Audience = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE");
    op.RequireHttpsMetadata = false;
    //Using JWKS validation
    op.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        ValidIssuer = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
        ValidAudience = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => {
            var httpClient = new HttpClient();
            var res = httpClient.GetStringAsync(DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY") + "/.well-known/jwks.json").Result;
            var jwks_keys = new JsonWebKeySet(res).Keys;
            return jwks_keys;
        },
        ClockSkew = TimeSpan.FromSeconds(1),
        RequireSignedTokens = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors((op) => {
    op.AddDefaultPolicy(
        policy => {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders(["WWW-Authenticate"]);
        }
    );
});
var app = builder.Build();

var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
var report = await healthCheckService.CheckHealthAsync();

if (report.Status == HealthStatus.Healthy){
    await using var db_write_dataSource = NpgsqlDataSource.Create(DotNetEnv.Env.GetString("DBWRITE_CONNECTION_STRING"));
    await using var db_read_dataSource = NpgsqlDataSource.Create(DotNetEnv.Env.GetString("DBREAD_CONNECTION_STRING"));
    app.UseRouting();
    app.UseCors();
    if (app.Environment.IsDevelopment()){
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Petro application APIs v1");
            });
            app.MapPut("/signup", async () => {
                await using var cmd = db_write_dataSource.CreateCommand($@"
                    INSERT INTO {DotNetEnv.Env.GetString("SCHEMA")}.user(username, password, padding) VALUES (@username, @password, @padding);
                ");
                while (true){
                    var hashed = PasswordHasher.Hash(new User{Username = "",Password = ""},"admin123");
                    var padding = hashed[1];
                    var hashedPassword = hashed[0];
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "username", Value = "mqthanggg"});
                    cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "password", Value = hashedPassword});
                    cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "padding", Value = padding});
                    int res = -1;
                    try{
                        res = await cmd.ExecuteNonQueryAsync();
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
                            return Results.Json(new {why = column},statusCode: 400);
                        }
                        return Results.Json(new {why = e.Message}, statusCode:400);
                    }
                    break;
                }
                return Results.Ok();
            });
        }
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHealthChecks("/health");
    app.MapGet("/dispenser/station/{id}", [Authorize]  async ([FromRoute] int id) => {
        string schemaName = DotNetEnv.Env.GetString("SCHEMA");
        await using var cmd = db_read_dataSource.CreateCommand($@"
        SELECT dp.name, f.price, f.long_name, f.short_name FROM
        {schemaName}.dispenser as dp
        INNER JOIN {schemaName}.fuel as f ON f.fuel_id = dp.fuel_id AND dp.station_id = @id
        ");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "id", Value = id});
        var res = await cmd.ExecuteReaderAsync();
        var dataTable = new DataTable();
        dataTable.Load(res);
        await res.CloseAsync();
        return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    });
    app.MapGet("/tank/station/{id}", [Authorize]  async ([FromRoute] int id) => {
        string schemaName = DotNetEnv.Env.GetString("SCHEMA");
        await using var cmd = db_read_dataSource.CreateCommand($@"
        SELECT t.name, f.short_name, t.max_volume FROM
        {schemaName}.tank as t
        INNER JOIN {schemaName}.fuel as f ON f.fuel_id = t.fuel_id AND t.station_id = @id
        ");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "id", Value = id});
        var res = await cmd.ExecuteReaderAsync();
        var dataTable = new DataTable();
        dataTable.Load(res);
        await res.CloseAsync();
        return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    });
    app.MapDelete("/station/{id}", [Authorize] async (int id) => {
        await using var cmd = db_write_dataSource.CreateCommand($@"
        DELETE FROM {DotNetEnv.Env.GetString("SCHEMA")}.station 
        WHERE station_id = @stationId
        ");
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "stationId", Value = id});
        var res = await cmd.ExecuteNonQueryAsync();
        if (res > 0) 
            return Results.Ok();
        return Results.BadRequest();
    });
    app.MapPut("/station/{id}", [Authorize] async (int id, Station body) => {
        await using var cmd = db_write_dataSource.CreateCommand($@"
        UPDATE {DotNetEnv.Env.GetString("SCHEMA")}.station
        SET 
        name = @name,
        address = @address
        WHERE
        station_id = @stationId
        ");
        cmd.Parameters.Clear();
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "stationId", Value = id});
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "name", Value = body.Name});
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "address", Value = body.Address});
        var res = await cmd.ExecuteNonQueryAsync();
        if (res > 0)
            return Results.Ok();
        return Results.BadRequest();
    });
    app.MapGet("/stations", [Authorize] async () => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT station_id,name,address FROM {DotNetEnv.Env.GetString("schema")}.station");
        var res = await cmd.ExecuteReaderAsync();
        var dataTable = new DataTable();
        dataTable.Load(res);
        await res.CloseAsync();
        return JsonConvert.SerializeObject(dataTable,Formatting.Indented);
    });
    app.MapPost("/login", async ([FromBody] User body) => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT user_id,username,password,padding FROM {DotNetEnv.Env.GetString("schema")}.user WHERE username = @username");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "username", Value = body.Username});
        var res = await cmd.ExecuteReaderAsync();
        if (!res.HasRows){
            await res.CloseAsync();
            return Results.Unauthorized();
        }
        res.Read();
        var encryptedPassword = res["password"].ToString() ?? "";
        var passwordPadding = res["padding"].ToString() ?? "";
        var userId = Convert.ToInt32(res["user_id"]);
        if (PasswordHasher.Verify(body,body.Password + passwordPadding,encryptedPassword)){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Sid, userId.ToString()),
                new Claim(ClaimTypes.Name, res["username"].ToString() ?? "")
            };
            var creds = new SigningCredentials(new RsaSecurityKey(RSAClass.LoadPrivateRsaKey()),SecurityAlgorithms.RsaSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
                audience: DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );
            using var refreshTokenCmd = db_write_dataSource.CreateCommand($@"
                UPDATE {DotNetEnv.Env.GetString("SCHEMA")}.user
                SET 
                refresh_token = @refreshToken,
                token_padding = @refreshTokenPadding,
                token_expired_time = now() + INTERVAL '7 days'
                WHERE
                user_id = @userId
            ");
            string randomRefreshToken;
            using (var _rng = RandomNumberGenerator.Create()){
                byte[] randomBytes = new byte[16];
                _rng.GetBytes(randomBytes);
                randomRefreshToken = Convert.ToBase64String(randomBytes);
            }
            res.Close();
            int col;
            while (true){
                List<string> hash = PasswordHasher.Hash(new {},randomRefreshToken);
                string hashedRefreshToken = hash[0];
                string refreshTokenPadding = hash[1];
                refreshTokenCmd.Parameters.Clear();
                refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "refreshToken", Value = hashedRefreshToken});
                refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "refreshTokenPadding", Value = refreshTokenPadding});
                refreshTokenCmd.Parameters.Add(new NpgsqlParameter{ParameterName = "userId", Value = userId});
                try{
                    col = await refreshTokenCmd.ExecuteNonQueryAsync();
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
                        return Results.Json(new {why = column},statusCode: 400);
                    }
                    return Results.Json(new {why = e.Message}, statusCode:400);
                }
                break;
            }
            if (col > 0){
                return Results.Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                    refresh_token = randomRefreshToken
                });
            }
        }
        return Results.Unauthorized();
    });
    app.MapPost("/refresh", async ([FromHeader(Name = "Authorization")] string authHeader, Token body) => {
        var claims = (List<Claim>)new JwtSecurityTokenHandler().ReadJwtToken(authHeader.Substring(7)).Claims;
        var userId = claims.First(e => e.Type == ClaimTypes.Sid).Value;
        var username = claims.First(e => e.Type == ClaimTypes.Name).Value;
        await using var cmd = db_read_dataSource.CreateCommand($@"
            SELECT username, refresh_token, token_padding, token_expired_time FROM {DotNetEnv.Env.GetString("SCHEMA")}.user
            WHERE user_id = @userId
        ");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "userId", Value = Convert.ToInt32(userId)});
        var res = await cmd.ExecuteReaderAsync();
        res.Read();
        if (!(res.HasRows && res["username"].ToString() == username))
            return Results.Unauthorized();
        var refreshTokenExpiredTime = Convert.ToDateTime(res["token_expired_time"]);
        var hashedRefreshToken = res["refresh_token"].ToString() ?? "";
        var refreshTokenPadding = res["token_padding"].ToString();
        if (
            DateTime.Now.CompareTo(refreshTokenExpiredTime) > 0 || //Expired
            !PasswordHasher.Verify(body, body.RefreshToken + refreshTokenPadding, hashedRefreshToken) //Invalid refresh token
        ) 
            return Results.Unauthorized();
        await res.CloseAsync();
        var creds = new SigningCredentials(new RsaSecurityKey(RSAClass.LoadPrivateRsaKey()),SecurityAlgorithms.RsaSha256);
        var tokenDescriptor = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds
        );
        return Results.Json(new {token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)}, statusCode: 200);
    });
    //Configure route for JWKS
    app.MapGet("/.well-known/jwks.json",() => {
        var rsa = RSAClass.LoadPublicRsaKey();
        var parameters = rsa.ExportParameters(false);
        var jwk = new JsonWebKey{
            Kty = "RSA",
            Alg= SecurityAlgorithms.RsaSha256,
            Kid= "rsa_public_key",
            E= Base64UrlEncoder.Encode(parameters.Exponent),
            N= Base64UrlEncoder.Encode(parameters.Modulus),
            Use= "sig",
        };
        return Results.Json(new {keys = new[]{jwk}});
    });
    app.Run();
}
else{
    await app.StopAsync();
}