public class Dispenser : Entity {
    public int? DispenserId{get; set;} = null;
    public int? StationId{get; set;} = null;
    public int? TankId{get; set;} = null;
    public int? FuelId{get; set;} = null;
    public int? Name{get; set;} = null;
    public List<Log>? Logs{get; set;}
}