using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(c => c.SingleLine = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1",new OpenApiInfo{Title = "Petro application API", Description = "List of APIs for petro application", Version = "v1"});
});
builder.Services.AddDbContextPool<DbWrite>((sp,options) => {
    options.UseNpgsql(DotNetEnv.Env.GetString("DBWRITE_CONNECTION_STRING")).UseSnakeCaseNamingConvention(); 
});
builder.Services.AddDbContextPool<DbRead>((sp,options) => {
    options.UseNpgsql(DotNetEnv.Env.GetString("DBREAD_CONNECTION_STRING")).UseSnakeCaseNamingConvention(); 
});
var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var db_read = scope.ServiceProvider.GetRequiredService<DbRead>();
var canConnect = await db_read.Database.CanConnectAsync();
app.Logger.LogInformation("Can connect to database as reader: {0}",canConnect);
var db_write = scope.ServiceProvider.GetRequiredService<DbWrite>();
canConnect = await db_write.Database.CanConnectAsync();
app.Logger.LogInformation("Can connect to database as writer: {0}",canConnect);

if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Petro application APIs v1");
    });
}

app.MapGet("/", () => "Hello World!");
app.MapHealthChecks("/health");
app.Run();
