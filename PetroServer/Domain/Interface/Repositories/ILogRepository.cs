using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface ILogRepository : IRepository<Log>
{
    Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity);
    Task<int> UpdateLogTimeAsync(LogResponse entity);
    Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetPageLogByStationIdAsync(Station entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetFullNullConditionAsync(GetFullNullConditionFilterResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetFullLogConditionAsync(GetFullNullConditionFilterResponse entity, int page, int pageSize);
}
