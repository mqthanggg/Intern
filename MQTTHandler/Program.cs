Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthCheckService();
builder.Services.AddDbService();
builder.Services.AddSignalRService();
builder.Services.AddMqttService();

DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
var report = await healthCheckService.CheckHealthAsync();

if (report.Status == HealthStatus.Healthy){
    app.MapController();
    app.MapHealthChecks("/health");
    app.Run();
}
else{
    await app.StopAsync();
}