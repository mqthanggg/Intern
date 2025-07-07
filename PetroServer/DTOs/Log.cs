public class LogResponse
{
    public required string Name { get; set; } = "";
    public required string FuelName { get; set; } = "";
    public required float TotalLiters { get; set; } = -1.0f;
    public required int Price { get; set; } = -1;
    public required int TotalAmount { get; set; } = -1;
    public int? LogType { get; set; } = -1;
    public required DateTime Time { get; set; }
}
