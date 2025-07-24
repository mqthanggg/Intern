public interface IStationRepository : IRepository<Station>
{
    Task<IReadOnlyList<StationResponse>> GetAllStationResponseAsync();
    Task<int> GetSumStationResponseAsync();
    Task<StationResponse> GetStationById(Station entity);
}