public static class DbInjection{
    public static IServiceCollection DbSetup(this IServiceCollection services){
        services.AddSingleton<IDbReadConnection,DbReadConnection>();
        services.AddSingleton<IDbWriteConnection,DbWriteConnection>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IStationRepository,StationRepository>();
        services.AddScoped<ILogRepository,LogRepository>();
        services.AddScoped<IDispenserRepository,DispenserRepository>();
        services.AddScoped<ITankRepository,TankRepository>();

        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IRevenueRepository, RevenueRepository>();
        services.AddScoped<IFuelRepository, FuelRepository>();
    return services;
    }
}