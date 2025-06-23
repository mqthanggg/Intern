public class Hasher : IHasher{
    private readonly PasswordHasher<object> _ph;
    private readonly RandomNumberGenerator _rng;

    public Hasher(){
        _ph = new PasswordHasher<object>();
        _rng = RandomNumberGenerator.Create();
    }
    public (string,string) Hash(object obj, string password){
        var padding = new byte[16];
        _rng.GetBytes(padding);
        return (_ph.HashPassword(obj,password+Convert.ToBase64String(padding)),Convert.ToBase64String(padding));
    }

    public bool Verify(object obj, string inPassword, string hashedPassword){
        return 
            _ph.VerifyHashedPassword(obj, hashedPassword, inPassword) == PasswordVerificationResult.Success ||
            _ph.VerifyHashedPassword(obj, hashedPassword, inPassword) == PasswordVerificationResult.SuccessRehashNeeded;
    }
}