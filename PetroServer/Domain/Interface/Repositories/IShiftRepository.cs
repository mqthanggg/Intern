public interface IShiftRepository : IRepository<Shift>
{
    Task<IReadOnlyList<ShiftResponse>> GetAllShiftResponseAsync();
    Task<IReadOnlyList<ShiftResponse>> GetShiftById(Shift shift);
}