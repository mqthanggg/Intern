public interface IRevenueRepository
{
   Task<SumRevenueResponse> GetTotalRevenueAsync();
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameShiftAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeShiftAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameYearAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeYearAsync(GetIdRevenue ren);
}
