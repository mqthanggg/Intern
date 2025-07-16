public interface IRevenueRepository
{
   //Task<SumRevenueResponse> GetTotalRevenueShiftAsync();
   Task<SumRevenueResponse> GetTotalRevenueAsync();
   Task<IReadOnlyList<SumStationResponse>> GetTotalRevenueStationtAsync();
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByType7dayAsync();
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueFullBynameAsync();
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueFullByTypeAsync();
   Task<IReadOnlyList<SumRevenueByDateResponse>> GetTotalRevenueByDayAsync();
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameShiftAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeShiftAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameYearAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeYearAsync(GetIdRevenue ren);
}
