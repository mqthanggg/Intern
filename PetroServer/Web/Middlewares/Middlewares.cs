public static class Middlewares{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder){
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
        applicationBuilder.UseHealthChecks("/health");
        return applicationBuilder;
    }
}