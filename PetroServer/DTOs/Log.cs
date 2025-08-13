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

public class GetFullNullConditionFilterResponse 
{
    public int? StationId { get; set; } = -1;
    public int? Name { get; set; } = -1;
    public string? FuelName { get; set; } = "";
    public int? LogType { get; set; } = -1;
    public DateTime? FromDate { get; set; } = null;
    public DateTime? ToDate { get; set; } = null;
    public int? FromPrice { get; set; } = -1;
    public int? ToPrice { get; set; } = -1;
    public int? FromAmount { get; set; } = -1;
    public int? ToAmount { get; set; } = -1;
    public int? FromLiter { get; set; } = -1;
    public int? ToLiter { get; set; } = -1;
}