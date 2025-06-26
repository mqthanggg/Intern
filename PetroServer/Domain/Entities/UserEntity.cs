public class User : Entity{
    public int UserId{get; set;} = -1;
    public string Username{get; set;} = "";
    public string Password{get; set;} = "";
    public string Padding{get; set;} = "";
    public string? RefreshToken{get; set;}
    public string? TokenPadding{get; set;}
    public DateTime? TokenExpiredTime{get; set;}
}