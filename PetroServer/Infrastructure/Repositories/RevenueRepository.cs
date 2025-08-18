
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

    public async Task<IReadOnlyList<SumStationResponse>> GetTotalRevenueByStationAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumStationResponse> revenue = (await connection.QueryAsync<SumStationResponse>(RevenueQueries.SumRevenueStation, ren)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByDateResponse>> GetTotalRevenueStationDayAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByDateResponse> revenue = (await connection.QueryAsync<SumRevenueByDateResponse>(RevenueQueries.SumRevenueStationDay, ren)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueStationByMonthResponse>> GetTotalRevenueStationMonthAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueStationByMonthResponse> revenue = (await connection.QueryAsync<SumRevenueStationByMonthResponse>(RevenueQueries.SumRevenueStationMonth, ren)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueStationByYearResponse>> GetTotalRevenueStationYearAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueStationByYearResponse> revenue = (await connection.QueryAsync<SumRevenueStationByYearResponse>(RevenueQueries.SumRevenueStationYear, ren)).ToList();
            return revenue;
        }
    }
    public async Task<RevenueResponse> GetTotalRevenueAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            var result = await connection.QueryFirstOrDefaultAsync<RevenueResponse>(RevenueQueries.SumRevenue);
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
    //=================================
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
    public async Task<IReadOnlyList<SumRevenueStation7DayResponse>> GetTotalRevenueByDayAsync()
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueStation7DayResponse> revenue = (await connection.QueryAsync<SumRevenueStation7DayResponse>(RevenueQueries.SumRevenueByDay)).ToList();
            return revenue;
        }
    }
    //=================================
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameDayAsync(GetIdRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameDay, ren)).ToList();
            return revenue;
        }
    }
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetDayAsync(GetDateRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameGetDay, ren)).ToList();
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
    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetDayAsync(GetDateRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeGetDay, ren)).ToList();
            return revenue;
        }
    }
    //=================================
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetMonthAsync(GetMonthRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameGetMonth, ren)).ToList();
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
    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetMonthAsync(GetMonthRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeGetMonth, ren)).ToList();
            return revenue;
        }
    }
    //=================================
    public async Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetYearAsync(GetYearRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByNameResponse> revenue = (await connection.QueryAsync<SumRevenueByNameResponse>(RevenueQueries.SumFuelbyNameGetYear, ren)).ToList();
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
    public async Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetYearAsync(GetYearRevenue ren)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<SumRevenueByTypeResponse> revenue = (await connection.QueryAsync<SumRevenueByTypeResponse>(RevenueQueries.SumFuelbyTypeGetYear, ren)).ToList();
            return revenue;
        }
    }
    //=================================
    public async Task<int> InsertAsync(User user)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(UserQuery.InsertUser, user);
            return affectedRows;
        }
    }

}