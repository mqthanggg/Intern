public static class Middlewares{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder){
        applicationBuilder.UseRouting();
        applicationBuilder.UseCors();
        applicationBuilder.UseAuthentication();
        applicationBuilder.Use(async (context, next) => {
            var antiForgery = context.Features.Get<IAntiforgeryValidationFeature>();
            if (antiForgery != null && !antiForgery.IsValid){
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("XSRF token validation failed!");
                return;
            }
            else{
                await next();
            }
        });
        applicationBuilder.UseAuthorization();
        applicationBuilder.UseAntiforgery();
        applicationBuilder.UseHealthChecks("/health");
        applicationBuilder.UseWebSockets();
        return applicationBuilder;
    }
}