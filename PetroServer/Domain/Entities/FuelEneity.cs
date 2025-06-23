public class Fuel : Entity {
    public required int FuelId {get; set;} = -1;
    public required string ShortName {get; set;} = "";
    public required string LongName {get; set;} = "";
    public required int Price{get; set;} = -1;
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}