using System.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
    app.Logger.LogInformation("Hashed password: {}",PasswordHasher.Hash(new User{Username="",Password=""},"admin123"));
    app.UseRouting();
    app.UseCors();
    if (app.Environment.IsDevelopment()){
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Petro application APIs v1");
            });
        }
    app.UseAuthorization();
    app.MapHealthChecks("/health");
    app.MapGet("/stations", async () => {
        await using var cmd = db_read_dataSource.CreateCommand($"SELECT * FROM {DotNetEnv.Env.GetString("schema")}.station");
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
            return Results.Json(new {why = "username"}, statusCode: 401);
        }
        res.Read();
        var encryptedPassword = res["password"].ToString() ?? "";
        if (PasswordHasher.Verify(body,body.Password,encryptedPassword)){
            return Results.Ok();
        }
        else{
            return Results.Json(new {why = "password"}, statusCode: 401);
        }        
    });
    app.Run();
}
else{
    await app.StopAsync();
}