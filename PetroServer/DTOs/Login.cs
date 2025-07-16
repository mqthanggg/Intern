public class LoginRequest{
    public required string Username{get; set;}
    public required string Password{get; set;}
}

public class LoginResponse
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required string Role {get; set;}
}

public class RegisterRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}