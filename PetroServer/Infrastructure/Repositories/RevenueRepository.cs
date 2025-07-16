
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
            return result;
        }
    }
    
    public async Task<IReadOnlyList<SumStationResponse>> GetTotalRevenueStationtAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumStationResponse> revenue = (await connection.QueryAsync<SumStationResponse>(RevenueQueries.SumRevenueByStation)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueFullBynameAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumRevenueFullByName)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueFullByTypeAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumRevenueFullByType)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByType7dayAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumRevenueBytype7day)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByDateResponse>> GetTotalRevenueByDayAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByDateResponse> revenue = (await connection.QueryAsync<SumRevenueByDateResponse>(RevenueQueries.SumRevenueByDay)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameShiftAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyName, ren)).ToList();
            return revenue;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeShiftAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyType, ren)).ToList();
            return revenue;
        }
    }

     public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameDayAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameDay, ren)).ToList();
            return revenue;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeDayAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeDay, ren)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameMonthAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameMonth, ren)).ToList();
            return revenue;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeMonthAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeMonth, ren)).ToList();
            return revenue;
        }
    }

     public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameYearAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameYear, ren)).ToList();
            return revenue;
        }
    }

    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeYearAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeYear, ren)).ToList();
            return revenue;
        }
    }

    public async Task<int> InsertAsync(User user)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(UserQuery.InsertUser, user);
            return affectedRows;
        }
    }
}