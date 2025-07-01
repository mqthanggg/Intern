public class StaffRepository : IStaffRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public StaffRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<int> InsertAsync(Staff entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StaffQuery.InsertStaff,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Staff entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StaffQuery.UpdateStaff,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Staff entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StaffQuery.DeleteStaff,entity);
            return affectedRows;
        }
    }
}