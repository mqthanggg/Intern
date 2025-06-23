public class StationUpdateRequest{
    public required int StationId {get; set;} = -1;
    public required string Name{get; set;} = "";
    public required string Address{get; set;} = "";
}

public class StationResponse{
    public required int StationId{get; set;} = -1;
    public required string Name{get; set;} = "";
    public required string Address{get; set;} = "";
}