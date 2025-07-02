public class StaffRepository : IStaffRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public StaffRepository(
        IDbWriteConnection dbWriteConnection,
        IDbReadConnection dbReadConnection
    )
    {
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }
    public async Task<IReadOnlyList<StaffResponse>> GetStaffById(Staff entity){
        await using (var connection = dbRead.CreateConnection()) {
            List<StaffResponse> Staff = (await connection.QueryAsync<StaffResponse>(StaffQuery.SelectStaffById,entity)).ToList();
            return Staff;
        }
    }
    public async Task<IReadOnlyList<StaffResponse>> GetAllStaffResponseAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<StaffResponse> staff = (await connection.QueryAsync<StaffResponse>(StaffQuery.SelectStaff)).ToList();
            return staff;
        }
    }
    
    public async Task<int> InsertAsync(Staff entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(StaffQuery.InsertStaff, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Staff entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(StaffQuery.UpdateStaff, entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Staff entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(StaffQuery.DeleteStaff, entity);
            return affectedRows;
        }
    }
    
}