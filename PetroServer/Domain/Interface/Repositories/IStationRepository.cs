public interface IStationRepository : IRepository<Station>
{
    Task<IReadOnlyList<StationResponse>> GetAllStationResponseAsync();
}