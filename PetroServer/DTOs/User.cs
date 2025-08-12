public class UserResponse{
    public required int UserId {get; set;} = 0;
    public required int Role{get; set;} = 0;
    public required string Username{get; set;} = "";
    public required bool Active{get; set;} = false;
}

public class UserResponseWithPage{
    public required IReadOnlyList<UserResponse> Users{get; set;} = [];
    public required int PageNumber{get; set;} = 0;
}

public class UserChangePasswordRequest{
    public required string NewPassword{get; set;} = "";
    public required string ReTypePassword{get; set;} = "";
    public required string OldPassword{get; set;} = "";
}

public class UserSignUpRequest{
    public required int Role {get; set;} = 0; 
    public required string Username {get; set;} = "";
    public required string Password {get; set;} = "";
    public required string ReTypePassword {get; set;} = "";
}

public class UserRemoveRequest{
    public required string Password {get; set;} = "";
}

public class UserRequestFilter{
    public required string? Username {get; set;} = null;
    public required int? Role {get; set;} = null;
    public required int? Active {get; set;} = null;
}

public class UserRequestFilterWithPagination{
    public required string? Username {get; set;} = null;
    public required int? Role {get; set;} = null;
    public required int? Active {get; set;} = null;
    public int Limit {get; set;} = 0;
    public int Offset {get; set;} = 0;
}