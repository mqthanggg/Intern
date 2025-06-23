public class Dispenser : Entity {
    public required int DispenserId{get; set;} = -1;
    public required Station Station{get; set;}
    public required Tank Tank{get; set;}
    public required Fuel Fuel{get; set;}
    public required int Name{get; set;}
    public List<Log>? Logs{get; set;}
}