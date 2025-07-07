public class LogUpdateService : ILogUpdateService{
    private readonly ILogRepository _logRepository;
    public LogUpdateService(
        ILogRepository logRepository
    ){
        _logRepository = logRepository;
    }
}