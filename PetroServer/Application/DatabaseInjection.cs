public static class DbInjection{
    public static IServiceCollection DbSetup(this IServiceCollection services){
        services.AddScoped<IDbRead,DBRead>();
        services.AddScoped<IDbWrite,DBWrite>();
        services.AddTransient<IAuditService,AuditService>();
        return services;
    }
}