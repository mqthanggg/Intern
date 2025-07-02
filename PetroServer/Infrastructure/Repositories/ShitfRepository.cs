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

    public async Task<IReadOnlyList<ShiftResponse>> GetAllShiftResponseAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<ShiftResponse> shifts = (await connection.QueryAsync<ShiftResponse>(ShiftQuery.SelectShift)).ToList();
            return shifts;
        }
    }
    
    public async Task<IReadOnlyList<ShiftResponse>> GetShiftById(Shift entity){
        await using (var connection = dbRead.CreateConnection()) {
            List<ShiftResponse> Shift = (await connection.QueryAsync<ShiftResponse>(ShiftQuery.SelectShiftById,entity)).ToList();
            return Shift;
        }
    }

    public async Task<int> InsertAsync(Shift entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(ShiftQuery.InsertShift, entity);
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