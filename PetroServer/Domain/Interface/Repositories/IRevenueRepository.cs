public interface IRevenueRepository
{
   Task<SumRevenueResponse> GetTotalRevenueAsync();
   Task<IReadOnlyList<SumRevenueByNameResponse>> GetTotalRevenueByNameAsync(GetIdRevenue ren);
   Task<IReadOnlyList<SumRevenueByTypeResponse>> GetTotalRevenueByTypeAsync(GetIdRevenue ren);
}
