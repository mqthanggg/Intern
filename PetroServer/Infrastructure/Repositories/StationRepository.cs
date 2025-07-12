public class StationRepository : IStationRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    private readonly IUsernameService username;
    public StationRepository(IDbWriteConnection dbWriteConnection, IDbReadConnection dbReadConnection, IUsernameService usernameService){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
        username = usernameService;
    }
    public async Task<int> GetSumStationResponseAsync(){
        await using (var connection = dbRead.CreateConnection()){
            int count = (await connection.ExecuteScalarAsync<int>(StationQuery.SelectSumStation));
            return count;
        }
    }
    public async Task<IReadOnlyList<StationResponse>> GetAllStationResponseAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<StationResponse> stations = (await connection.QueryAsync<StationResponse>(StationQuery.SelectStation)).ToList();
            return stations;
        }
    }

    public async Task<int> InsertAsync(Station entity){
        entity.CreatedBy ??= username.GetUsername();
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.InsertStation,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Station entity){
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.UpdateStation,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Station entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.DeleteStation,entity);
            return affectedRows;
        }
    }
}