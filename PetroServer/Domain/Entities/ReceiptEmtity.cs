public class Receipt : Entity
{
    public int? ReceiptId { get; set; } = null;
    public string? ReceiptDate { get; set; } = null;
    public string? SupplierId { get; set; } = null;
    public string? StationId { get; set; } = null;
    public string? TotalAmount { get; set; } = null;
}
