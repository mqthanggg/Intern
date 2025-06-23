public class AsymmetricKeyService : IAsymmetricKeyService{
    private readonly string _privateKeyPath;
    private readonly string _publicKeyPath;

    public AsymmetricKeyService(){
        _privateKeyPath = Env.GetString("PRIVATE_KEY_PATH");
        _publicKeyPath = Env.GetString("PUBLIC_KEY_PATH");
    }
    public RSA LoadPrivateRsaKey(){
        var rsa = RSA.Create();
        var pemContent = File.ReadAllText(_privateKeyPath);
        rsa.ImportFromPem(pemContent);
        return rsa;
    }
    public RSA LoadPublicRsaKey(){
        var rsa = RSA.Create();
        var pemContent = File.ReadAllText(_publicKeyPath);
        rsa.ImportFromPem(pemContent);
        return rsa;
    }
}
