public class Station : Entity{
    public required int StationId{get; set;} = -1;
    public required string Name{get; set;} = "";
    public required string Address{get; set;} = "";
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}