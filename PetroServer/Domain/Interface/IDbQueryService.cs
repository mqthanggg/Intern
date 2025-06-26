public interface IDbQueryService{
    Task<object> ExecuteQueryAsync<TInput, TOutput>(TInput obj, DbOperation operation) 
        where TInput : Entity
        where TOutput : Entity;
}