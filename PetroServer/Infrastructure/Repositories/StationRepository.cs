public class StationRepository : IStationRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public StationRepository(IDbWriteConnection dbWriteConnection, IDbReadConnection dbReadConnection){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<StationResponse>> GetAllStationResponseAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<StationResponse> stations = (await connection.QueryAsync<StationResponse>(StationQuery.SelectStation)).ToList();
            return stations;
        }
    }

    public async Task<int> InsertAsync(Station entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.InsertStation,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Station entity){
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