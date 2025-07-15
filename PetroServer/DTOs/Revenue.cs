
public class SumRevenueResponse
{
    public decimal? TotalLiters { get; set; } = -1;
    public decimal? TotalRevenue { get; set; } = -1;
    public decimal? TotalImport { get; set; } = -1;
    public decimal? TotalProfit { get; set; } = -1;
}

public class SumStationResponse
{
    public required int StationId { get; set; } = -1;
    public required string StationName { get; set; } = "";
    public decimal? TotalLiters { get; set; } = -1;
    public decimal? TotalRevenue { get; set; } = -1;
    public decimal? TotalImport { get; set; } = -1;
    public decimal? TotalProfit { get; set; } = -1;
}

public class SumRevenueResponseByShift
{
    public required string ShiftNow { get; set; } = "";
    public decimal? TotalAmount { get; set; } = -1;
    public decimal? TotalLiters { get; set; } = -1;
}

public class SumRevenueByNameResponse
{
     public required string FuelName { get; set; } = "";
    public decimal? TotalAmount { get; set; } = -1;
    public decimal? TotalLiters { get; set; } = -1;
}

public class SumRevenueByTypeResponse
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
    public decimal? TotalAmount { get; set; } = null;
    public decimal? TotalLiters { get; set; } = null;

}
