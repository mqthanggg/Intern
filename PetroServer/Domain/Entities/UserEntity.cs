public class User : Entity{
    public int? UserId{get; set;} = null;
    public string? Username{get; set;} = null;
    public string? Password{get; set;} = null;
    public int? Role{get; set;} = null;
    public string? Padding{get; set;} = null;
    public string? RefreshToken{get; set;} = null;
    public string? TokenPadding{get; set;} = null;
    public DateTime? TokenExpiredTime{get; set;}
}