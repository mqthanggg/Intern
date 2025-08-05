public static class DependencyInjection{
    public static IServiceCollection AddServices(this IServiceCollection services){
        services.AddHttpContextAccessor();
        services.AddSingleton<IJWTService,JWTService>();
        services.AddSingleton<IAsymmetricKeyService,AsymmetricKeyService>();
        services.AddSingleton<IJWKsService,JWKsService>();
        services.AddSingleton<IHasher,Hasher>();
        services.AddSingleton<IUsernameService,UsernameService>();
        services.AddSingleton<ILogUpdateService,LogUpdateService>();
        services.AddScoped<IWebSocketHubService,WebSocketHub>();
        return services;
    }
}