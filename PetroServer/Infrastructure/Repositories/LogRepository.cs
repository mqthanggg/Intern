public class LogRepository : ILogRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public LogRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<Log>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<Log> logs = (await connection.QueryAsync<Log>(LogQuery.SelectLog)).ToList();
            return logs;
        }
    }

    public async Task<Log> GetByIdAsync(Log entity){
        await using (var connection = dbRead.CreateConnection()){
            Log log = await connection.QuerySingleAsync<Log>(LogQuery.SelectLogById, entity);
            return log;
        }
    }

    public async Task<int> InsertAsync(Log entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(LogQuery.InsertLog,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Log entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(LogQuery.UpdateLog,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Log entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(LogQuery.DeleteLog,entity);
            return affectedRows;
        }
    }
    public async Task<object> GetAsync<TInput>(TInput entity) where TInput : Entity{
        if (entity is Log log){
            return log.LogId == -1 ?
                await GetAllAsync() :
                await GetByIdAsync(log);
        }
        else if(entity is Station station){
            await using (var connection = dbRead.CreateConnection()){
                List<Log> logs = (await connection.QueryAsync<Log>(LogQuery.SelectLogByStationId,station)).ToList();
                return logs;
            }
        }
        throw new InvalidOperationException("Invalid input type");
    }
}