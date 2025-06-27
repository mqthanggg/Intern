public static class DbInjection{
    public static IServiceCollection DbSetup(this IServiceCollection services){
        services.AddSingleton<IDbReadConnection,DbReadConnection>();
        services.AddSingleton<IDbWriteConnection,DbWriteConnection>();
        services.AddTransient<IDbQueryService,DbQueryService>();
        services.AddScoped<IRepositoryFactory,RepositoryFactory>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IStationRepository,StationRepository>();
        services.AddScoped<ILogRepository,LogRepository>();
        services.AddScoped<IDispenserRepository,DispenserRepository>();
        services.AddScoped<ITankRepository,TankRepository>();
        return services;
    }
}