public class FuelResponse
{
    public required int FuelId { get; set; } = -1;
    public required string ShortName { get; set; } = "";
    public required string LongName { get; set; } = "";
    public required int Price { get; set; } = 0;
    public List<DispenserResponse>? Dispensers { get; set; }
    public List<TankResponse>? Tanks { get; set; }
}