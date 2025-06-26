public class Dispenser : Entity {
    public int DispenserId{get; set;} = -1;
    public int StationId{get; set;} = -1;
    public int TankId{get; set;} = -1;
    public int FuelId{get; set;} = -1;
    public int Name{get; set;} = -1;
    public List<Log>? Logs{get; set;}
}