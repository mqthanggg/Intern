public class AssignmentRepository : IAssignmentRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public AssignmentRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }
    public async Task<IReadOnlyList<AssignmentResponse>> GetAllAssignmentResponseAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<AssignmentResponse> assignments = (await connection.QueryAsync<AssignmentResponse>(AssignmentQuery.SelectAssignment)).ToList();
            return assignments;
        }
    }

    public async Task<int> InsertAsync(Assignment entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(AssignmentQuery.InsertAssignment, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Assignment entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(AssignmentQuery.UpdateAssignment,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Assignment entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(AssignmentQuery.DeleteAssignment,entity);
            return affectedRows;
        }
    }
}