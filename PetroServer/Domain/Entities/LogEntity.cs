public class Log : Entity
{
    public int? LogId { get; set; } = null;
    public int? DispenserId { get; set; } = null;
    public string? FuelName { get; set; } = null;
    public float? TotalLiters { get; set; } = null;
    public int? TotalAmount { get; set; } = null;
    public int? LogType { get; set; } = null;
    public DateTime? Time { get; set; } = null;
}