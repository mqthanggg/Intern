public class Station : Entity{
    public int StationId{get; set;} = -1;
    public string Name{get; set;} = "";
    public string Address{get; set;} = "";
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}