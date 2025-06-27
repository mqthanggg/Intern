public class TankRepository : ITankRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public TankRepository(
        IDbWriteConnection dbWriteConnection, 
        IDbReadConnection dbReadConnection
    ){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<Tank>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<Tank> tanks = (await connection.QueryAsync<Tank>(TankQuery.SelectTank)).ToList();
            return tanks;
        }
    }

    public async Task<Tank> GetByIdAsync(Tank entity){
        await using (var connection = dbRead.CreateConnection()){
            Tank tank = await connection.QuerySingleAsync<Tank>(TankQuery.SelectTankById, entity);
            return tank;
        }
    }

    public async Task<int> InsertAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.InsertTank,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.UpdateTank,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Tank entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(TankQuery.DeleteTank,entity);
            return affectedRows;
        }
    }
    public async Task<object> GetAsync<TInput>(TInput entity) where TInput : Entity{
        if (entity is Tank tank){
            return tank.TankId == -1 ?
                await GetAllAsync() :
                await GetByIdAsync(tank);
        }
        else if(entity is Station station){
            await using (var connection = dbRead.CreateConnection()){
                List<TankResponse> logs = (await connection.QueryAsync<TankResponse>(TankQuery.SelectTankByStationId,station)).ToList();
                return logs;
            }
        }
        throw new InvalidOperationException("Invalid input type");
    }
}