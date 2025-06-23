public interface IJWTService{
    string GenerateAccessToken(int userId, string username);
}