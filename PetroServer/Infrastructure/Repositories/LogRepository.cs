public class LogRepository : ILogRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    private readonly IUsernameService username;
    public LogRepository(
        IDbWriteConnection dbWriteConnection,
        IDbReadConnection dbReadConnection,
        IUsernameService usernameService
    )
    {
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
        username = usernameService;
    }
    public async Task<int> InsertAsync(Log entity)
    {
        entity.CreatedBy ??= username.GetUsername();
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.InsertLog, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Log entity)
    {
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.UpdateLog, entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Log entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.DeleteLog, entity);
            return affectedRows;
        }
    }
    public async Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<LogResponse> logs = (await connection.QueryAsync<LogResponse>(LogQuery.SelectLogByStationId, entity)).ToList();
            return logs;
        }
    }
    public async Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<LogResponse> logs = (await connection.QueryAsync<LogResponse>(LogQuery.SelectFullLogByStationId, entity)).ToList();
            return logs;
        }
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetPageLogByStationIdAsync(Station entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPageLogByStationId, new
            {
                StationId = entity.StationId,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

        return (logs.ToList(), totalCount);
    }

    public async Task<int> UpdateLogTimeAsync(LogResponse entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.UpdateLogTime, entity);
            return affectedRows;
        }
    }
    //==========================================
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserNameAsync(GetDispenserResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserNameByStationId, new
            {
                StationId = entity.StationId,
                Name= entity.Name,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

        return (logs.ToList(), totalCount);
    }
   
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameAsync(GetFuelResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelNameByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }

    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeAsync(GetLogTypeResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogTypeByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDateAsync(GetDateRevenue entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDateByStationId, new
            {
                StationId = entity.StationId,
                Time = entity.Time.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAsync(GetPeriodResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        // Lấy dữ liệu phân trang
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodByStationId, new
            {
                StationId = entity.StationId,
                toDate= entity.ToDate.Date,
                fromDate= entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
   
}
