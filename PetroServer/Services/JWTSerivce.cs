public class JWTService : IJWTService{
    private readonly SigningCredentials _creds;
    private readonly string _authority;
    private readonly string _audience;
    private readonly bool _isDevelopment;
    public JWTService(IWebHostEnvironment _env, IAsymmetricKeyService _key){
        _isDevelopment = _env.IsDevelopment();
        _creds = new SigningCredentials(new RsaSecurityKey(_key.LoadPrivateRsaKey()), SecurityAlgorithms.RsaSha256);
        _authority = Env.GetString(_isDevelopment ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY");
        _audience = Env.GetString(_isDevelopment ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE");
    }
    public string GenerateAccessToken(int userId, string username, string role){
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Sid, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _authority,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: _creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    public bool Verify(string token){
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuers = Env.GetString(_isDevelopment ? "DEVELOPMENT_VALID_ISSUERS" : "PRODUCTION_VALID_ISSUERS").Split(";").ToArray(),
            ValidAudience = Env.GetString(_isDevelopment ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE"),
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => {
                var httpClient = new HttpClient();
                var jwks_keys = httpClient.GetFromJsonAsync<IEnumerable<JsonWebKey>>(Env.GetString(_isDevelopment ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY") + "/.well-known/jwks.json").Result;
                return jwks_keys;
            },
            ClockSkew = TimeSpan.FromSeconds(0),
            RequireSignedTokens = true
        };
        try{
            jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters,out var _);
            return true;
        }
        catch(SystemException e){
            Console.WriteLine($"Token validation failed, why: {e.Message}");
            return false;
        }
    }
}