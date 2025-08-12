public class LogOutRequest{
    public required string Token {get; set;} = "";
}

public class TokenRequest{
    public required string Token {get; set;} = "";
    public required string RefreshToken{get; set;} = "";
}

public class TokenResponse{
    public required string Token {get; set;} = "";
}