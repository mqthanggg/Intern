public class UserRepository : IUserRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    private readonly IUsernameService usernameService;
    public UserRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection,
        IUsernameService username    
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
        usernameService = username;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<User> users = (await connection.QueryAsync<User>(UserQuery.SelectUser)).ToList();
            return users;
        }
    }

    public async Task<User> GetUserByIdAsync(User entity){
        await using (var connection = dbRead.CreateConnection()){
            User user = await connection.QuerySingleAsync<User>(UserQuery.SelectUserById, entity);
            return user;
        }
    }

    public async Task<int> InsertAsync(User entity){
        entity.CreatedBy ??= usernameService.GetUsername();
        entity.LastModifiedBy ??= entity.CreatedBy;
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(UserQuery.InsertUser,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(User entity){
        entity.LastModifiedBy ??= usernameService.GetUsername();
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
    public async Task<IReadOnlyList<UserResponse>> GetUserWithActiveAsync(UserRequestFilterWithPagination userRequest){
        await using (var connection = dbRead.CreateConnection()){
            List<UserResponse> users = (await connection.QueryAsync<UserResponse>(UserQuery.GetUserWithActive,userRequest)).ToList();           
            return users;
        }
    }
    public async Task<int> UpdateUserPasswordAsync(User entity){
        entity.LastModifiedBy ??= usernameService.GetUsername();
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(UserQuery.UpdateUserPassword,entity);
            return affectedRows;
        }
    }
    public async Task<int> GetUserCountWithFilterAsync(UserRequestFilter filter){
        await using (var connection = dbWrite.CreateConnection()){
            int Count = await connection.ExecuteScalarAsync<int>(UserQuery.GetUserCount,filter);
            return Count;
        }
    }
}