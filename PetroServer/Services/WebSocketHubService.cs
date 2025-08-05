public class WebSocketHub : IWebSocketHubService{
    private readonly HubConnection _connection;
    public WebSocketHub(){
        _connection = new 
        HubConnectionBuilder().
        WithUrl(Env.GetString("MQTT_HANDLER_URL")).
        Build();
    }
    public async Task JoinDevice(string device, int id){
        await _connection.InvokeAsync("JoinDevice", device, id);
    }
    public async Task StartAsync(){
        await _connection.StartAsync();
    }
    public async Task StopAsync(){
        await _connection.StopAsync();
    }
    public HubConnection GetHubConnection(){
        return _connection;
    }
}