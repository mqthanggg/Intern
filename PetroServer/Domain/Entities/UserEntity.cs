public class User : Entity{
    public required int UserId{get; set;} = -1;
    public required string Username{get; set;} = "";
    public required string Password{get; set;} = "";
    public required string Padding{get; set;} = "";
    public string? RefreshToken{get; set;}
    public string? TokenPadding{get; set;}
    public DateTime? TokenExpiredTime{get; set;}
}