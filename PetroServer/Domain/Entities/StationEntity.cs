public class Station : Entity{
    public int? StationId{get; set;} = null;
    public string? Name{get; set;} = null;
    public string? Address{get; set;} = null;
    public List<Dispenser>? Dispensers{get; set;}
    public List<Tank>? Tanks{get; set;}
}