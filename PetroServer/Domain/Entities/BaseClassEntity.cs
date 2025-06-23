public class Entity{
    public required string CreatedBy {get; set;} = "";
    public required DateTime CreatedDate {get; set;} = default;
    public required string LastModifiedBy{get; set;} = "";
    public required DateTime LastModifiedDate{get; set;} = default;
}