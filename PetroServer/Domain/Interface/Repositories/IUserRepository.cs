
public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByUsernameAsync(User entity);
    Task<User> GetUserTokenAsync(User entity);
    Task<IReadOnlyList<UserResponse>> GetUserWithActiveAsync(UserRequestFilterWithPagination userRequest);
    Task<int> UpdateUserPasswordAsync(User entity);
    Task<User> GetUserByIdAsync(User entity);
    Task<int> GetUserCountWithFilterAsync(UserRequestFilter filter);
}
