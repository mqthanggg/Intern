public static class JSONInjection{
    public static IServiceCollection JSONSetup(this IServiceCollection services){
        services.ConfigureHttpJsonOptions(op => {
            op.SerializerOptions.WriteIndented = true;
            op.SerializerOptions.IncludeFields = true;
        });
        return services;
    }
}