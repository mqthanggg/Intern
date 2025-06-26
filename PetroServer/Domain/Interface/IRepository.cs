public interface IRepository<T> where T : Entity{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(T entity);
    Task<int> InsertAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity); 
    Task<object> GetAsync<TInput>(TInput entity) where TInput : Entity;
}