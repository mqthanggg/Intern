public class AssignmentResponse
{
    public required int AssignmentId { get; set; } = -1;
    public required int ShiftType { get; set; } = -1;
    public required int StaffId { get; set; } = -1;
    public required string StaffName { get; set; } = "";
    public required int StationId { get; set; } = -1;
    public DateTime WorkDate { get; set; }
}

public class AssignmentRequest{
    public required int StationId {get; set;} = -1;
    public required DateTime WorkDate {get; set;} = default;
}