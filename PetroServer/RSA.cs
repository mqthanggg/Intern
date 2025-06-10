using System.Security.Cryptography;

static class RSAClass{
    private static readonly string privateKeyPath = "rsa_key/private_key.pem";
    private static readonly string publicKeyPath = "rsa_key/public_key.pem";
    public static RSA LoadPrivateRsaKey(){
        var rsa = RSA.Create();
        var pemContent = File.ReadAllText(privateKeyPath);
        rsa.ImportFromPem(pemContent);
        return rsa;
    }
    public static RSA LoadPublicRsaKey(){
        var rsa = RSA.Create();
        var pemContent = File.ReadAllText(publicKeyPath);
        rsa.ImportFromPem(pemContent);
        return rsa;
    }
}