
public interface IUserRepository : IRepository<User>{
    Task<User> GetUserLoginAsync(string username);
}