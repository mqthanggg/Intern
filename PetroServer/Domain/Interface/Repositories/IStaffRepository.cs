public interface IStaffRepository : IRepository<Staff>
{
    Task<IReadOnlyList<StaffResponse>> GetAllStaffResponse();
    Task<IReadOnlyList<StaffResponse>> GetStaffById(Staff staff);
}