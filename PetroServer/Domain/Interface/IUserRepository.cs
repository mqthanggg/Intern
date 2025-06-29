
public interface IUserRepository : IRepository<User>{
    Task<User> GetUserByUsernameAsync(User entity);
}