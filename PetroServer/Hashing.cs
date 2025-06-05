using Microsoft.AspNetCore.Identity;
public static class PasswordHasher{
    private static readonly PasswordHasher<User> ph = new PasswordHasher<User>();
    public static string Hash(User user, string password){
        return ph.HashPassword(user,password);
    }

    public static bool Verify(User user, string inPassword, string hashedPassword){
        return 
            ph.VerifyHashedPassword(user, hashedPassword, inPassword) == PasswordVerificationResult.Success ||
            ph.VerifyHashedPassword(user, hashedPassword, inPassword) == PasswordVerificationResult.SuccessRehashNeeded;
    }
}