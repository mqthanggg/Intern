public class DispenserRepository : IDispenserRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public DispenserRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<int> InsertAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.InsertDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.UpdateDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.DeleteDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<IReadOnlyList<DispenserResponse>> GetDispensersByStationIdAsync(Station entity){
        await using (var connection = dbRead.CreateConnection()){
            List<DispenserResponse> dispensers = (await connection.QueryAsync<DispenserResponse>(DispenserQuery.GetDispenserByStationId,entity)).ToList();
            return dispensers;
        }
    }
}