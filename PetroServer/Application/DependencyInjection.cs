public static class DependencyInjection{
    public static IServiceCollection AddServices(this IServiceCollection services){
        services.AddHttpContextAccessor();
        services.AddLogging();
        services.AddSingleton<IJWTService,JWTService>();
        services.AddSingleton<IAsymmetricKeyService,AsymmetricKeyService>();
        services.AddSingleton<IJWKsService,JWKsService>();
        services.AddSingleton<IHasher,Hasher>();
        services.AddSingleton<IUsernameService,UsernameService>();
        services.AddSingleton<IMqttService,MqttService>();
        services.AddSingleton<ILogUpdateService,LogUpdateService>();
        services.AddHostedService(e => (MqttService)e.GetRequiredService<IMqttService>());
        return services;
    }
}