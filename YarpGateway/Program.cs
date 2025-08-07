
var builder = WebApplication.CreateBuilder(args);
// var modelBuilder = new ODataConventionModelBuilder();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("customPolicy", builder => builder.Expire(TimeSpan.FromSeconds(20)));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true);
    });
});

// modelBuilder.EntitySet<Log>("log");
// builder.Services.AddControllers().AddOData(
//     options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
//     .AddRouteComponents(routePrefix: "odata", model: modelBuilder.GetEdmModel()));

var app = builder.Build();
app.UseCors("AllowAll");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy(proxyPipeline =>
    {
        proxyPipeline.UseWebSockets();
    });
});

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});
app.MapReverseProxy();
app.Run();
