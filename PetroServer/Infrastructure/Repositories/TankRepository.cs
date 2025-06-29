public class TankRepository : ITankRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public TankRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }
    public async Task<int> InsertAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.InsertTank,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.UpdateTank,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.DeleteTank,entity);
            return affectedRows;
        }
    }
    public async Task<IReadOnlyList<TankResponse>> GetTanksByStationIdAsync(Station entity){
        await using (var connection = dbRead.CreateConnection()){
            List<TankResponse> logs = (await connection.QueryAsync<TankResponse>(TankQuery.SelectTankByStationId,entity)).ToList();
            return logs;
        }
    }
}