public interface IJWTService{
    /// <summary>
    /// Generate a JWT from the given UID, user name and its role.
    /// All will be stored as claims
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="username"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    string GenerateAccessToken(int userId, string username, string role);
    /// <summary>
    /// Validate the JWT token (without validating lifetime)
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    bool Verify(string token);
}