public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<IReadOnlyList<AssignmentResponse>> GetAllAssignmentResponseAsync();
    Task<IReadOnlyList<AssignmentResponse>> GetAllAssignmentResponseByStationIdAsync(Assignment entity);
}