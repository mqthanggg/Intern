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
    public string GenerateAccessToken(int userId, string username){
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Sid, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _authority,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: _creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    public IReadOnlyList<Claim> GetClaims(string bearer){
        if (bearer[..7] != "Bearer "){
            throw new InvalidDataException("Invalid bearer token: must starts with \"Bearer <token>\"");
        }
        return new JwtSecurityTokenHandler().ReadJwtToken(bearer[7..]).Claims.ToList();
    }
    public bool Verify(string token){
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            ValidIssuer = Env.GetString(_isDevelopment ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY"),
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