
public class RevenueRepository : IRevenueRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    public RevenueRepository(
        IDbWriteConnection dbWriteConnection,
        IDbReadConnection dbReadConnection
    )
    {
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
    }

    public async Task<SumRevenueResponse> GetTotalRevenueAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            var result = await connection.QueryFirstOrDefaultAsync<SumRevenueResponse>(RevenueQueries.SumRevenue);
            result.TotalAmount ??= 0;
            return result;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyName, ren)).ToList();
            return revenue;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyType, ren)).ToList();
            return revenue;
        }
    }
}