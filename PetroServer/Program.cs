Env.Load();
var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.AddLogging();
builder.Services.JSONSetup();
builder.Services.AuthSetup(env);
builder.Services.AddEndpointsApiExplorer();
builder.Services.HealthCheckSetup();
builder.Services.SwaggerSetup();
builder.Services.DbSetup();
builder.Services.AddServices();
builder.Logging.AddSimpleConsole(c => c.SingleLine = true);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
var report = await healthCheckService.CheckHealthAsync();

if (report.Status == HealthStatus.Healthy){
    
    app.UseMiddlewares();
    if (app.Environment.IsDevelopment()){
        IdentityModelEventSource.ShowPII = true; 
        app.Swagger();
        app.MapSignup();
    }
    app.MapPublicController();
    // app.MapReverseProxy();
    app.MapReport();
    app.Run();
}
else{
    await app.StopAsync();
}