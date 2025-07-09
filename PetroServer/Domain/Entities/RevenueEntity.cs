public class Revenue : Entity
{
    public string? BuoiHienTai { get; set; } = null;
    public decimal? TongDoanhThu { get; set; } = null;
    public decimal? TongNhienLieu { get; set; } = null;
}
public class GetIdRevenue
{
    public int StationId { get; set; }
}
public class RevenueByName : Entity
{
    public string? FuelName { get; set; } = null;
    public decimal? TongDoanhThu { get; set; } = null;
    public decimal? TongNhienLieu { get; set; } = null;
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
                4 => "Trả trước"
            };
        }
    }
    public decimal? TongDoanhThu { get; set; } = null;
    public decimal? TongNhienLieu { get; set; } = null;

}
