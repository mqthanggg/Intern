public class AssignmentResponse
{
    public required int AssignmentId { get; set; } = -1;
    public required int StaffId { get; set; } = -1;
    public required int ShiftId { get; set; } = -1;
    public required int StationId { get; set; } = -1;
    public DateTime WorkDate { get; set; }
}