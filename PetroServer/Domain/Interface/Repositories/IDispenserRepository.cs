public interface IDispenserRepository : IRepository<Dispenser>{
    Task<IReadOnlyList<DispenserResponse>> GetDispensersByStationIdAsync(Station entity);
    Task<int> GetDispenserFuelIdAsync(Dispenser entity);
}