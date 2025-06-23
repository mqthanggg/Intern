public interface IAuditService{
    Task<int> ExecuteWriteQueryWithUpdateAuditAsync(NpgsqlCommand command, string tableName, DbOperation operation);
}