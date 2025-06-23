public class Tank : Entity{
    public required int TankId{get; set;} = -1;
    public required Fuel Fuel{get; set;}
    public required Station Station{get; set;}
    public required int Name{get; set;} = -1;
    public required int MaxVolume{get; set;} = -1;
}