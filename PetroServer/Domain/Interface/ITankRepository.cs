public interface ITankRepository : IRepository<Tank>{
    Task<IReadOnlyList<TankResponse>> GetTanksByStationIdAsync(Station entity);
}