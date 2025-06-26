public class StationRepository : IStationRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public StationRepository(IDbWriteConnection dbWriteConnection, IDbReadConnection dbReadConnection){
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<IReadOnlyList<Station>> GetAllAsync(){
        await using (var connection = dbRead.CreateConnection()){
            List<Station> stations = (await connection.QueryAsync<Station>(StationQuery.SelectStation)).ToList();
            return stations;
        }
    }

    public async Task<Station> GetByIdAsync(Station entity){
        await using (var connection = dbRead.CreateConnection()){
            Station station = await connection.QuerySingleAsync<Station>(StationQuery.SelectStationById, entity);
            return station;
        }
    }

    public async Task<int> InsertAsync(Station entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.InsertStation,entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Station entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.UpdateStation,entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Station entity){
        await using (var connection = dbWrite.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(StationQuery.DeleteStation,entity);
            return affectedRows;
        }
    }

    public async Task<object> GetAsync<TInput>(TInput entity) where TInput : Entity{
        if (entity is Station station){
            return station.StationId == -1 ?
                await GetAllAsync() :
                await GetByIdAsync(station);
        }
        throw new InvalidOperationException("Invalid input type");
    }
}