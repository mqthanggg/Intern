public static class DependencyInjection{
    public static IServiceCollection AddDbService(this IServiceCollection services){
        services.AddSingleton<IDbWriteConnection,DbWriteConnection>();
        services.AddSingleton<ILogUpdateService,LogUpdateService>();
        services.AddScoped<ILogRepository,LogRepository>();
        return services;
    }
    public static IServiceCollection AddMqttService(this IServiceCollection services){
        services.AddSingleton<IMqttService,MqttService>();
        services.AddHostedService(e => (MqttService)e.GetRequiredService<IMqttService>());
        return services;
    }

    public static IServiceCollection AddHealthCheckService(this IServiceCollection services){
        services.AddHealthChecks()
        .AddNpgSql(
            Env.GetString("DBWRITE_CONNECTION_STRING"),
            name:"write"
        );
        return services;
    }
}