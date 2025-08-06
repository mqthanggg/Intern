
public class RevenueResponse
{
    public decimal? TotalLiters { get; set; } = -1;
    public decimal? TotalRevenue { get; set; } = -1;
    public decimal? TotalImport { get; set; } = -1;
    public decimal? TotalProfit { get; set; } = -1;
}
public class SumStationResponse: RevenueResponse
{
    public required int StationId { get; set; } = -1;
    public required string StationName { get; set; } = "";
}
public class SumRevenueResponse {
    public decimal? TotalAmount { get; set; } = -1;
    public decimal? TotalLiters { get; set; } = -1;
}

public class SumRevenueResponseByShift: SumRevenueResponse
{
    public required string ShiftNow { get; set; } = "";
}

public class SumRevenueByNameResponse: SumRevenueResponse
{
    public required string FuelName { get; set; } = "";
}

public class SumRevenueByTypeResponse: SumRevenueResponse
{
    public int? LogType { get; set; } = null;
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
}

public class SumRevenueByDateResponse : SumStationResponse
{
    public required string Date { get; set; } = "";
}

public class SumRevenueStationByDateResponse : SumStationResponse
{
    public required string Date { get; set; } = "";
}

public class SumRevenueStationByMonthResponse : SumStationResponse
{
    public required string Month { get; set; } = "";
}

public class SumRevenueStationByYearResponse : SumStationResponse
{
    public required string Year { get; set; } = "";
}
