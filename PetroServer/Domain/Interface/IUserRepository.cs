
public interface IUserRepository : IRepository<User>{
    Task<User> GetUserLoginAsync(User entity);
}