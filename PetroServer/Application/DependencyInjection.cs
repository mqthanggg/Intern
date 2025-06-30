public static class DependencyInjection{
    public static IServiceCollection AddServices(this IServiceCollection services){
        services.AddSingleton<IJWTService,JWTService>();
        services.AddSingleton<IAsymmetricKeyService,AsymmetricKeyService>();
        services.AddSingleton<IJWKsService,JWKsService>();
        services.AddSingleton<IHasher,Hasher>();
        services.AddLogging();
        return services;
    }
}