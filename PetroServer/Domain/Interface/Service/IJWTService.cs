public interface IJWTService{
    string GenerateAccessToken(int userId, string username);
    IReadOnlyList<Claim> GetClaims(string bearer);
    bool Verify(string token);
}