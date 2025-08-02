using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("customPolicy", builder => builder.Expire(TimeSpan.FromSeconds(20)));
});
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .SetIsOriginAllowed(_ => true);
    });
});
var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120)
};
// app.UseWebSockets(webSocketOptions);
// app.UseCors("AllowAll");
app.UseWebSockets();
app.MapReverseProxy();
app.Run();
