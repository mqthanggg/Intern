using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
        ValidAudience = DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
        // IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => {
        //     var httpClient = new HttpClient();
        //     var res = httpClient.GetStringAsync(DotNetEnv.Env.GetString(builder.Environment.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY") + "/.well-known/jwks.json").Result;
        //     var jwks_keys = new JsonWebKeySet(res).Keys;
        //     return jwks_keys;
        // }
        IssuerSigningKey = new RsaSecurityKey(RSAClass.LoadPublicRsaKey()),
        RequireSignedTokens = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors((op) => {
    op.AddDefaultPolicy(
        policy => {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
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
        }
    app.UseAuthorization();
    app.UseAuthentication();
    app.MapHealthChecks("/health");
    app.MapGet("/station/{id}", async (int id) => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT name,address FROM {DotNetEnv.Env.GetString("schema")}.station WHERE station_id = @id");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "id", Value = id});
        var res = await cmd.ExecuteReaderAsync();
        res.Read();
        return Results.Ok(new {name = res["name"],address = res["address"]});
    });
    app.MapGet("/stations", async () => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT station_id,name,address FROM {DotNetEnv.Env.GetString("schema")}.station");
        var res = await cmd.ExecuteReaderAsync();
        var dataTable = new DataTable();
        dataTable.Load(res);
        return JsonConvert.SerializeObject(dataTable,Formatting.Indented);
    });
    app.MapPost("/login", async (User body) => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT * FROM {DotNetEnv.Env.GetString("schema")}.user WHERE username = @username");
        cmd.Parameters.Add(new NpgsqlParameter{ParameterName = "username", Value = body.Username});
        var res = await cmd.ExecuteReaderAsync();
        if (!res.HasRows){
            await res.CloseAsync();
            return Results.Unauthorized();
        }
        res.Read();
        var encryptedPassword = res["password"].ToString() ?? "";
        var passwordPadding = res["padding"].ToString() ?? "";
        if (PasswordHasher.Verify(body,body.Password + passwordPadding,encryptedPassword)){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Sid, res["user_id"].ToString() ?? ""),
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
            return Results.Ok(new {token= new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)});
        }
        else{
            return Results.Unauthorized();
        }        
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