public class ReceiptResponse
{
    public required int ReceiptId { get; set; } = -1;
    public required string ReceiptDate { get; set; } = string.Empty;
    public required string SupplierId { get; set; } = string.Empty;
    public required string StationId { get; set; } = string.Empty;
    public required string TotalImport { get; set; } = string.Empty;
}
