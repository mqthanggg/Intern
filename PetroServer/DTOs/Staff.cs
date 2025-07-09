public class StaffResponse
{
    public required int StaffId { get; set; } = -1;
    public required string StaffName { get; set; } = "";
    public DateTime DateBirth { get; set; }
    public required string Phone { get; set; } = "";
    public required string Address { get; set; } = "";
    public required string Email { get; set; } = "";
}