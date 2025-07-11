public class UsernameService : IUsernameService{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IJWTService _jwt;
    public UsernameService(
        IHttpContextAccessor httpContext,
        IJWTService jWTService
    ){
        _httpContext = httpContext;
        _jwt = jWTService;
    }
    public string GetUsername(){
        string bearer = _httpContext.HttpContext?.Request.Headers.Authorization.FirstOrDefault() ?? "";
        string username = _jwt.GetClaims(bearer).First(e => e.Type == ClaimTypes.Name).Value;
        return username;
    }
}