public static class HealthCheck{
    public static IServiceCollection HealthCheckSetup(this IServiceCollection services){
        services.AddHealthChecks()
        .AddNpgSql(Env.GetString("DBWRITE_CONNECTION_STRING"),name:"write").AddNpgSql(Env.GetString("DBREAD_CONNECTION_STRING"),name:"read");
        return services;
    }
}