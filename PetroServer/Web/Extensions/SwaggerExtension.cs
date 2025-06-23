public static class SwaggerExtension{
    public static IApplicationBuilder Swagger(this IApplicationBuilder applicationBuilder){
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI(c => {
            c.SwaggerEndpoint("/swagger/v1/swagger.json","Petro application APIs v1");
        });
        return applicationBuilder;
    }
}