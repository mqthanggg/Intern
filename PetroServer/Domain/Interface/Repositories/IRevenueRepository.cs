public interface IRevenueRepository
{
   Task<RevenueResponse> GetTotalRevenueAsync();
   Task<IReadOnlyList<SumStationResponse>> GetTotalRevenueStationtAsync();
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByType7dayAsync();
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueFullBynameAsync();
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueFullByTypeAsync();
   Task<IReadOnlyList<SumRevenueByDateResponse>> GetTotalRevenueByDayAsync();
   Task<IReadOnlyList<SumStationResponse>> GetTotalRevenueByStationAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByDateResponse>> GetTotalRevenueStationDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueStationByMonthResponse>> GetTotalRevenueStationMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueStationByYearResponse>> GetTotalRevenueStationYearAsync(GetIdRevenue ren);
   //=================================
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameShiftAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeShiftAsync(GetIdRevenue ren);
   //=================================
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetDayAsync(GetDateRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeDayAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetDayAsync(GetDateRevenue ren);
   //=================================
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeMonthAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetMonthAsync(GetMonthRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetMonthAsync(GetMonthRevenue ren);
   //=================================
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameYearAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNamegetYearAsync(GetYearRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeYearAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeGetYearAsync(GetYearRevenue ren);
}
