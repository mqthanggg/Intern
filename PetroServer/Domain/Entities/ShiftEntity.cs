public class Shift : Entity
{
    public int? ShiftId { get; set; } = null;
    public int? ShiftName { get; set; } = null;
    public TimeSpan? StartTime { get; set; } = null;
    public TimeSpan? EndTime { get; set; } = null;
}