using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Web;
public static class PasswordHasher{
    private static readonly PasswordHasher<object> ph = new PasswordHasher<object>();
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
    public static List<string> Hash(object obj, string password){
        var padding = new byte[16];
        _rng.GetBytes(padding);
        return new List<string>{ph.HashPassword(obj,password+Convert.ToBase64String(padding)),Convert.ToBase64String(padding)};
    }

    public static bool Verify(object obj, string inPassword, string hashedPassword){
        return 
            ph.VerifyHashedPassword(obj, hashedPassword, inPassword) == PasswordVerificationResult.Success ||
            ph.VerifyHashedPassword(obj, hashedPassword, inPassword) == PasswordVerificationResult.SuccessRehashNeeded;
    }
}