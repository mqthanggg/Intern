public class PaginationSetting{
    public int Limit {get; set;} = 0;
    public int Offset {get; set;} = 0;
    public PaginationSetting(
        int limit,
        int page
    ){
        Limit = limit;
        Offset = (page-1)*limit;
    }
}