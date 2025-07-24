public class Revenue : Entity
{
    public decimal? TotalLiters { get; set; } = null;
    public decimal? TotalRevenue { get; set; } = null;
    public decimal? TotalImport { get; set; } = null;
    public decimal? TotalProfit { get; set; } = null;
}
public class SumStationRevenue: Entity 
{
    public required int StationId { get; set; } = -1;
    public required string StationName { get; set; } = "";
    public decimal? TotalLiters { get; set; } = null;
    public decimal? TotalRevenue { get; set; } = null;
    public decimal? TotalImport { get; set; } = null;
    public decimal? TotalProfit { get; set; } = null;
}
public class RevenueByShift : Entity
{
    public string? shiftnow { get; set; } = null;
    public decimal? TotalAmount { get; set; } = null;
    public decimal? TotalLiters { get; set; } = null;
    public decimal? TotalProfit { get; set; } = null;
}
public class GetIdRevenue
{
    public int StationId { get; set; }
}
public class GetDateRevenue
{
    public int StationId { get; set; }
    public DateTime Time { get; set; }
}
public class GetMonthRevenue
{
    public int StationId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
public class GetYearRevenue
{
    public int StationId { get; set; }
    public int Year { get; set; }
}
public class RevenueByName : Entity
{
    public string? FuelName { get; set; } = null;
    public decimal? TotalAmount { get; set; } = null;
    public decimal? TotalLiter { get; set; } = null;
}

public class RevenueByType : Entity
{
    public int LogType { get; set; }
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
