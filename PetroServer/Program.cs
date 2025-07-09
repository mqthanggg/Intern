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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowAll");
    app.UseWebSockets();
    app.Map("/ws/total_revenue_by_type/{id}", PublicController.GetSumRevenueByTypeWS);

    
    app.Run();
}
else{
    await app.StopAsync();
}