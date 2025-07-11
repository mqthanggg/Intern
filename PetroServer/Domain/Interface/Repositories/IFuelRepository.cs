public interface IFuelRepository : IRepository<Fuel>{
    Task<string> GetFuelShortNameByIdAsync(Fuel entity);
}