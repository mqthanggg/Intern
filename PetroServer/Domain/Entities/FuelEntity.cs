public class Fuel : Entity {
    public int? FuelId {get; set;} = null;
    public string? ShortName {get; set;} = null;
    public string? LongName {get; set;} = null;
    public int? Price{get; set;} = null;
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}