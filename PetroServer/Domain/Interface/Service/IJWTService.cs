public interface IJWTService{
    string GenerateAccessToken(int userId, string username, string role);
    bool Verify(string token);
}