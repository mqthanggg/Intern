public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<IReadOnlyList<AssignmentResponse>> GetAllAssignmentResponseAsync();
}