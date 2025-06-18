public class User{
    public required string Username{get; set;} = "";
    public required string Password{get; set;} = "";
}

public class Token{
    public required string RefreshToken{get; set;} = "";
}

public class Station{
    public required string Name{get; set;} = "";
    public required string Address{get; set;} = "";
}