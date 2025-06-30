public class UserRepository : IUserRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public UserRepository(IDbWriteConnection dbWriteConnection, IDbReadConnection dbReadConnection){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<User> users = (await connection.QueryAsync<User>(UserQuery.SelectUser)).ToList();
            return users;
        }
    }

    public async Task<User> GetByIdAsync(User entity){
        await using (var connection = dbRead.CreateConnection()){
            User user = await connection.QuerySingleAsync<User>(UserQuery.SelectUserById, entity);
            return user;
        }
    }

    public async Task<int> InsertAsync(User entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(UserQuery.InsertUser,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(User entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(UserQuery.UpdateUser,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(User entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(UserQuery.DeleteUser,entity);
            return affectedRows;
        }
    }
    public async Task<User> GetUserByUsernameAsync(User entity){
        await using (var connection = dbRead.CreateConnection()){
            User user = await connection.QuerySingleAsync<User>(UserQuery.GetUserByUsername,entity);
            return user;
        }
    }
    public async Task<User> GetUserTokenAsync(User entity){
        await using (var connection = dbRead.CreateConnection()){
            User user = await connection.QuerySingleAsync<User>(UserQuery.GetUserToken,entity);
            return user;
        }
    }
}