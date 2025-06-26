public class Log : Entity{
    public int LogId{get; set;} = -1;
    public int DispenserId{get; set;} = -1;
    public string FuelName{get; set;} = "";
    public float TotalLiters{get; set;} = -1.0f;
    public int TotalAmount{get; set;} = -1;
    public DateTime Time{get; set;} = default;
}