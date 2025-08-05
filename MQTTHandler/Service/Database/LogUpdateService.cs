namespace LogUpdate{
    public enum DispenserState{
        IDLE,
        PUMP,
        RESET
    }
}
public class LogUpdateService : ILogUpdateService{   
    private class WSDispenserRecord{
        public float liter {get; set;}
        public int price {get; set;}
        public DispenserState state{get; set;}
        public int payment {get; set;}
    }
    private readonly IServiceScopeFactory _factory;
    private ConcurrentDictionary<int,WSDispenserRecord> _currentData;
    public LogUpdateService(
        IServiceScopeFactory factory
    ){
        _factory = factory;
        _currentData = new ConcurrentDictionary<int, WSDispenserRecord>();
    }

    public Task SegmentProcessAsync(int id, ReadOnlySequence<byte> segment){
        byte[] data = segment.ToArray();
        return Task.Run(
            async () => {
                try
                {
                    WSDispenserRecord? wSDispenserRecord = JsonSerializer.Deserialize<WSDispenserRecord>(
                        new MemoryStream(data)
                    );
                    if (wSDispenserRecord != null){
                        await FetchDispenserData(
                            id,
                            wSDispenserRecord
                        );
                    }
                    }
                catch (Exception e)
                {  
                    Console.WriteLine($"Failed, why: {e}");                    
                }
            }
        );
    }
    private async Task FetchDispenserData(int id, WSDispenserRecord record){
        if (
            record.state == DispenserState.RESET &&
            _currentData.TryGetValue(id,out var value) &&
            value.state == DispenserState.PUMP
        ){
            try{
                var scope = _factory.CreateScope();
                var affectedRows = await scope.ServiceProvider.GetRequiredService<ILogRepository>().InsertAsync(
                    new Log{
                        DispenserId = id,
                        TotalLiters = value.liter,
                        TotalAmount = value.price,
                        LogType = record.payment,
                        CreatedBy = "server",
                        LastModifiedBy = "server"
                    }
                );
                if (affectedRows != 1){
                    throw new Exception("Unable to insert to Log");
                }
            }
            catch(Exception e){
                Console.WriteLine($"Failed, why: {e}");
            }
        }
        else if (
            record.state == DispenserState.PUMP
        ){
            lock(_currentData){
                _currentData.AddOrUpdate(
                    id,
                    record,
                    (_,_) => record
                );
            }
        }
        
    }
}