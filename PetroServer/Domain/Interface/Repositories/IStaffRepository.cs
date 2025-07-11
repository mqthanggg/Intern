public interface IStaffRepository : IRepository<Staff>
{
    Task<IReadOnlyList<StaffResponse>> GetAllStaffResponseAsync();
    Task<StaffResponse> GetStaffByIdAsync(Staff staff);
}