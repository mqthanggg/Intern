public class AuditService : IAuditService{
    private readonly NpgsqlDataSource _writeDs;
    public AuditService(IDbWrite dbWrite){
        _writeDs = dbWrite.DataSource;
    }
    public async Task<int> ExecuteWriteQueryWithUpdateAuditAsync(NpgsqlCommand command, string tableName, DbOperation operation){  
        //Return number of affected columns from the original query
        //Return -1 if there's any error.
        command.CommandText += $"\nRETURNING {tableName}_id";
        int? id = Convert.ToInt32(await command.ExecuteScalarAsync());
        string auditQueryCommand = operation switch
        {
            DbOperation.INSERT => $@"
                    UPDATE {Env.GetString("SCHEMA")}.{tableName} 
                    SET
                    created_by = @username,
                    created_date = now(),
                    last_modified_by = @username,
                    last_modified_date = now()
                    WHERE
                    {tableName}_id = @id
                ",
            DbOperation.UPDATE => $@"
                    UPDATE {Env.GetString("SCHEMA")}.{tableName} 
                    SET
                    last_modified_by = @username,
                    last_modified_date = now()
                    WHERE
                    {tableName}_id = @id
                ",
            _ => throw new InvalidOperationException("Unsupported operation type for write operations."),
        };
        await using var auditCmd = _writeDs.CreateCommand(auditQueryCommand);
        int auditRes = await auditCmd.ExecuteNonQueryAsync();
        if (auditRes == -1){
            throw new InvalidOperationException("Expecting at least 1 affected row.");
        }
        return auditRes;
    }
}