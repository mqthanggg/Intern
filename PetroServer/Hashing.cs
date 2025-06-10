using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Web;
public static class PasswordHasher{
    private static readonly PasswordHasher<User> ph = new PasswordHasher<User>();
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
    public static string Hash(User user, string password){
        var padding = new byte[16];
        _rng.GetBytes(padding);
        return ph.HashPassword(user,password+Convert.ToBase64String(padding));
    }

    public static bool Verify(User user, string inPassword, string hashedPassword){
        return 
            ph.VerifyHashedPassword(user, hashedPassword, inPassword) == PasswordVerificationResult.Success ||
            ph.VerifyHashedPassword(user, hashedPassword, inPassword) == PasswordVerificationResult.SuccessRehashNeeded;
    }
}