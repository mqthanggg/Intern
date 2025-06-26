public class JWKsService : IJWKsService{
    private readonly RSA _rsa;

    public JWKsService(){
        _rsa = new AsymmetricKeyService().LoadPublicRsaKey();
    }
    public List<JsonWebKey> GetJWKs(){
        var parameters = _rsa.ExportParameters(false);
        var jwk = new JsonWebKey{
            Kty = "RSA",
            Alg= SecurityAlgorithms.RsaSha256,
            Kid= "rsa_public_key",
            E= Base64UrlEncoder.Encode(parameters.Exponent),
            N= Base64UrlEncoder.Encode(parameters.Modulus),
            Use= "sig",
        };
        return [jwk];
    }
      public static IResult GetJWKs1()
    {
        var rsa = RSAClass.LoadPublicRsaKey();
        var parameters = rsa.ExportParameters(false);
        var jwk = new JsonWebKey
        {
            Kty = "RSA",
            Alg = SecurityAlgorithms.RsaSha256,
            Kid = "rsa_public_key",
            E = Base64UrlEncoder.Encode(parameters.Exponent),
            N = Base64UrlEncoder.Encode(parameters.Modulus),
            Use = "sig",
        };
        return Results.Json(new { keys = new[] { jwk } });
    }
}