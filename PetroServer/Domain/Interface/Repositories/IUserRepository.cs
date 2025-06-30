
public interface IUserRepository : IRepository<User>{
    Task<User> GetUserByUsernameAsync(User entity);
    Task<User> GetUserTokenAsync(User entity);
}