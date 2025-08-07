public class LogResponse
{
    public required string Name { get; set; } = "";
    public required string FuelName { get; set; } = "";
    public required float TotalLiters { get; set; } = -1.0f;
    public required int Price { get; set; } = -1;
    public required int TotalAmount { get; set; } = -1;
    public int? LogType { get; set; } = -1;
    public string LogTypeName
    {
        get
        {
            return LogType switch
            {
                1 => "Bán lẻ",
                2 => "Công nợ",
                3 => "Khuyến mãi",
                4 => "Trả trước",
                _ => "Không xác định"
            };
        }
    }
    public required DateTime Time { get; set; }
}

public class LogFilterResponse
{
    public int StationId { get; set; }
    public string? FuelName { get; set; }
    public string? DispenserName { get; set; }
    public DateTime? SelectedDate { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
