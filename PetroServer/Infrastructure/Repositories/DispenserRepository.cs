public class DispenserRepository : IDispenserRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public DispenserRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<Dispenser>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<Dispenser> dispensers = (await connection.QueryAsync<Dispenser>(DispenserQuery.SelectDispenser)).ToList();
            return dispensers;
        }
    }

    public async Task<Dispenser> GetByIdAsync(Dispenser entity){
        await using (var connection = dbRead.CreateConnection()){
            Dispenser dispenser = await connection.QuerySingleAsync<Dispenser>(DispenserQuery.SelectDispenserById, entity);
            return dispenser;
        }
    }

    public async Task<int> InsertAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.InsertDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.UpdateDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Dispenser entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(DispenserQuery.DeleteDispenser,entity);
            return affectedRows;
        }
    }
    public async Task<object> GetAsync<TInput>(TInput entity) where TInput : Entity{
        if (entity is Dispenser dispenser){
            return dispenser.DispenserId == -1 ?
                await GetAllAsync() :
                await GetByIdAsync(dispenser);
        }
        else if (entity is Station station){
            await using (var connection = dbRead.CreateConnection()){
                List<DispenserResponse> dispensers = (await connection.QueryAsync<DispenserResponse>(DispenserQuery.GetDispenserByStationId,station)).ToList();
                return dispensers;
            }
        }
        throw new InvalidOperationException("Invalid input type");
    }
}