public interface IShiftRepository : IRepository<Shift>
{
    Task<IReadOnlyList<ShiftResponse>> GetAllShiftResponseAsync();
    Task<ShiftResponse> GetShiftById(Shift entity);
}

