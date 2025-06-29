public static class Auth{
    public static IServiceCollection AuthSetup(this IServiceCollection services, IWebHostEnvironment _env){
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op => {
            op.Authority = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY");
            op.Audience = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE");
            op.RequireHttpsMetadata = !_env.IsDevelopment();
            op.IncludeErrorDetails = _env.IsDevelopment();
            //Using JWKS validation
            op.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuerSigningKey = true,
                ValidIssuer = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
                ValidAudience = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => {
                    var httpClient = new HttpClient();
                    var res = httpClient.GetStringAsync(Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY") + "/.well-known/jwks.json").Result;
                    var jwks_keys = new JsonWebKeySet(res).Keys;
                    return jwks_keys;
                },
                ClockSkew = TimeSpan.FromSeconds(0),
                RequireSignedTokens = true
            };
        });
        services.AddAuthorization();
        services.AddCors((op) => {
            op.AddDefaultPolicy(
                policy => {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders(["WWW-Authenticate"]);
                }
            );
        });
        return services;
    }
}