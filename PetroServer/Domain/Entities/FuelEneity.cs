public class Fuel : Entity {
    public int FuelId {get; set;} = -1;
    public string ShortName {get; set;} = "";
    public string LongName {get; set;} = "";
    public int Price{get; set;} = -1;
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}