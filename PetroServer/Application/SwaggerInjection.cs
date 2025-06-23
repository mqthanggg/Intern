using Microsoft.OpenApi.Models;

public static class Swagger{
    public static IServiceCollection SwaggerSetup(this IServiceCollection services){
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1",new OpenApiInfo{Title = "Petro application API", Description = "List of APIs for petro application", Version = "v1"});
            c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{
                Description = "JWT Authorization using Bearer header",
                Name = "JWT Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "JWT Authorization",
                        In = ParameterLocation.Header,},
                    new List<string>()
                }
            });
        });
        return services;
    }
}