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

//-----------------------------------------------
public class GetDispenserResponse
{
    public required int StationId { get; set; } = -1;
    public required int Name { get; set; } = -1;
}

public class GetFuelResponse
{
    public required int StationId { get; set; } = -1;
    public required string FuelName { get; set; } = "";
}

public class GetLogTypeResponse
{
    public required int StationId { get; set; } = -1;
    public required int LogType { get; set; } = -1;
}

public class GetPeriodResponse
{
    public required int StationId { get; set; } = -1;
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
}