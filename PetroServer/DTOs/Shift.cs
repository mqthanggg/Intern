public class ShiftResponse
{
    public required int ShiftId { get; set; } = -1;
    public required int ShiftType { get; set; } = -1;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class ShiftRevenueResponse
{
    public string ShiftName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; } = 0;
    public decimal TotalFuel { get; set; } = 0;
}