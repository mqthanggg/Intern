public interface ILogRepository{
    Task<int> InsertAsync(Log entity);
}