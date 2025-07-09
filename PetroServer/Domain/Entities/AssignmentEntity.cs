public class Assignment : Entity
{
    public int? AssignmentId { get; set; } = null;
    public int? StaffId { get; set; } = null;
    public int? ShiftId { get; set; } = null;
    public int? StationId { get; set; } = null;
    public DateTime? WorkDate { get; set; } = null;
}