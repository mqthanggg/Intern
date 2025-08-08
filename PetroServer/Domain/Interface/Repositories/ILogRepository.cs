public interface ILogRepository : IRepository<Log>
{
    Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity);
    Task<int> UpdateLogTimeAsync(LogResponse entity);
    Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetPageLogByStationIdAsync(Station entity, int page, int pageSize);
    //=============================================================
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserNameAsync(GetDispenserResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeAsync(GetLogTypeResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDateAsync(GetDateRevenue entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameAsync(GetFuelResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAsync(GetPeriodResponse entity, int page, int pageSize);
}
