public class JWTService : IJWTService{
    private SigningCredentials? _creds;
    private string? _authority;
    private string? _audience;

    public JWTService(IWebHostEnvironment _env, IAsymmetricKeyService _key){
        _creds = new SigningCredentials(new RsaSecurityKey(_key.LoadPrivateRsaKey()), SecurityAlgorithms.RsaSha256);
        _authority = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUTHORITY" : "PRODUCTION_AUTHORITY");
        _audience = Env.GetString(_env.IsDevelopment() ? "DEVELOPMENT_AUDIENCE" : "PRODUCTION_AUDIENCE");
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
            expires: DateTime.UtcNow.AddMinutes(10),
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
}