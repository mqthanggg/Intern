public class FuelRepository : IFuelRepository{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public FuelRepository(
        IDbWriteConnection write,
        IDbReadConnection read
    ){
        dbWrite = write;
        dbRead = read;
    }
    public async Task<int> InsertAsync(Fuel entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(FuelQuery.InsertFuel, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Fuel entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(FuelQuery.UpdateFuel, entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Fuel entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(FuelQuery.DeleteFuel, entity);
            return affectedRows;
        }
    }

    public async Task<string> GetFuelShortNameByIdAsync(Fuel entity){
        await using (var connection = dbRead.CreateConnection())
        {
            Fuel fuel = await connection.QuerySingleAsync<Fuel>(FuelQuery.SelectFuelShortNameById,entity);
            return fuel.ShortName ?? "";
        }
    }
}