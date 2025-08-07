public interface ILogRepository : IRepository<Log>
{
    Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity);
    Task<int> UpdateLogTimeAsync(LogResponse entity);
    Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity);
}
