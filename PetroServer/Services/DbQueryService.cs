public class DbQueryService : IDbQueryService{
    private readonly IRepositoryFactory _repoFactory;
    private readonly string _author;
    public DbQueryService(
        IRepositoryFactory factory,
        string author = "admin")
    {
        _author = author;
        _repoFactory = factory;
    }

    private T AuditUpdate<T>(T obj, DbOperation operation) where T : Entity{
        switch (operation){
            case DbOperation.UPDATE:
                obj.LastModifiedBy = _author;
                obj.LastModifiedDate = DateTime.UtcNow;
                break;
            case DbOperation.INSERT:
                obj.LastModifiedBy = _author;
                obj.LastModifiedDate = DateTime.UtcNow;
                obj.CreatedBy = _author;
                obj.CreatedDate = DateTime.Now;
                break;
        }
        return obj;
    }
    public async Task<object> ExecuteQueryAsync<TInput, TOutput>(TInput obj, DbOperation operation) 
        where TInput : Entity
        where TOutput : Entity{  
        //Updating audit fields if needed
        obj = AuditUpdate(obj,operation);
        IRepository<TOutput> repo = _repoFactory.GetRepository<TOutput>();
        switch (operation)
        {  
            case DbOperation.SELECT:
                return await repo.GetAsync(obj);
            case DbOperation.INSERT:
                if (obj is TOutput insertOutput)
                    return await repo.InsertAsync(insertOutput);
                break;
            case DbOperation.UPDATE:
                if (obj is TOutput updateOutput)
                    return await repo.UpdateAsync(updateOutput);
                break;
            case DbOperation.DELETE:
                if (obj is TOutput deleteOutput)
                    return await repo.DeleteAsync(deleteOutput);
                break;
        }
        throw new InvalidOperationException("Invalid operation");
    }
}