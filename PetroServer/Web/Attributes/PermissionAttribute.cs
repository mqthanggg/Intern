using Microsoft.AspNetCore.Mvc.Filters;

public class PermissionAttribute: AuthorizeAttribute, IAuthorizationFilter{
    private readonly string _permission;
    public PermissionAttribute(string permission){
        _permission = permission;
    }
    public void OnAuthorization(AuthorizationFilterContext context){
        var authorizationHeader = context.HttpContext.Request.Headers.Authorization.First() ?? "";
        if (authorizationHeader[..7] != "Bearer "){
            throw new InvalidDataException("Invalid bearer token");
        }
        var bearer = authorizationHeader[7..];
        var claims = context.HttpContext.User.Claims;
        if (
            claims.First(e => e.Type == ClaimTypes.Role).Value != _permission &&
            new JwtSecurityTokenHandler().ReadJwtToken(bearer).ValidTo.CompareTo(DateTime.UtcNow) >= 0
        ){
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        return;
    }
}