public class ShiftRepository : IShiftRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public ShiftRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<int> InsertAsync(Shift entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(ShiftQuery.InsertShift,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Shift entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(ShiftQuery.UpdateShift,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Shift entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(ShiftQuery.DeleteShift,entity);
            return affectedRows;
        }
    }
}