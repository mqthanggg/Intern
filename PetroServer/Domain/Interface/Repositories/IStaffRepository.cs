public interface IStaffRepository : IRepository<Staff>
{
    Task<IReadOnlyList<StaffResponse>> GetAllStaffResponseAsync();
    Task<IReadOnlyList<StaffResponse>> GetStaffById(Staff staff);
}