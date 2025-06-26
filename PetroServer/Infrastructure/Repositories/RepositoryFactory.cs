public class RepositoryFactory : IRepositoryFactory{
    private readonly Dictionary<Type, object> _repositoryDictionary;
    public RepositoryFactory(
        IUserRepository userRepository,
        IStationRepository stationRepository,
        ILogRepository logRepository
    ){
        _repositoryDictionary = new Dictionary<Type, object>{
            {typeof(User), userRepository},
            {typeof(Station), stationRepository},
            {typeof(Log),logRepository}
        };
    }
    public IRepository<T> GetRepository<T>() where T : Entity{
        if (_repositoryDictionary.TryGetValue(typeof(T),out var repo)){
            return (IRepository<T>)repo;
        }
        throw new NotSupportedException($"Invalid entity type");
    }
}