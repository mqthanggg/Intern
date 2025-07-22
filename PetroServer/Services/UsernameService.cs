public class UsernameService : IUsernameService{
    private readonly IHttpContextAccessor _httpContext;
    public UsernameService(
        IHttpContextAccessor httpContext
    ){
        _httpContext = httpContext;
    }
    public string GetUsername(){
        string username = (_httpContext.HttpContext?.User.Claims.First(e => e.Type == ClaimTypes.Name).Value) ?? throw new InvalidDataException("No username is found");
        return username;
    }
}