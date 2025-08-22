public interface IWebSocketHubService{
    Task StartAsync();
    Task StopAsync();
    HubConnection GetHubConnection();
    Task JoinDevice(string device, int id);
    Task LeftDevice(string device, int id);
}