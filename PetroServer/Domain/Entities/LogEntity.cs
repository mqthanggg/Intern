public class Log : Entity{
    public required int LogId{get; set;} = -1;
    public required Dispenser Dispenser{get; set;}
    public required string FuelName{get; set;}
    public required float TotalLiters{get; set;} = -1.0f;
    public required int TotalAmount{get; set;} = -1;
    public required DateTime Time{get; set;} = default;
}