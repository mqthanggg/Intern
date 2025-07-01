public class ShiftResponse
{
    public required int ShiftId { get; set; } = -1;
    public required int ShiftName { get; set; } = -1;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}