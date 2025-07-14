using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

public class Supplier : Entity
{
    public int? SupplierId { get; set; } = null;
    public string? SupplierName { get; set; } = null;
    public string? Phone { get; set; } = null;
    public string? Address { get; set; } = null;
    public string? Email { get; set; } = null;
}
