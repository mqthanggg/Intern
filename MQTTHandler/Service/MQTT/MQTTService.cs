using Microsoft.AspNetCore.SignalR;

public class DeviceMonitoringHub : Hub<DeviceMonitoringInterface>{
    public async Task SendMessageAsync(string channel, string message){
        await Clients.Group(channel).SendMessage(message);
    }
    public async Task JoinDevice(string device, int id){
        await Groups.AddToGroupAsync(Context.ConnectionId,$"{device}:{id}");
    }
}
public class MqttService : IHostedService{
    private class WSDispenserRecord{
        public float liter {get; set;}
        public int price {get; set;}
        public DispenserState state{get; set;}
        public int payment {get; set;}
    }
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;
    private readonly ILogUpdateService _logUpdate;
    private readonly IHubContext<DeviceMonitoringHub,DeviceMonitoringInterface> _hub;
    public MqttService(
        ILogUpdateService logUpdate,
        IHubContext<DeviceMonitoringHub,DeviceMonitoringInterface> hub
    ){
        _hub = hub;
        _logUpdate = logUpdate;
        var mqttFactory = new MqttClientFactory();
        _client = mqttFactory.CreateMqttClient();
        _options = new MqttClientOptionsBuilder().
            WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).
            WithTcpServer(
                Env.GetString("MOSQUITTO_HOST"),
                Convert.ToInt32(Env.GetString("MOSQUITTO_PORT"))
            ).
            WithClientId(Guid.NewGuid().ToString()).
            WithCredentials(
                Env.GetString("MOSQUITTO_USERNAME"),
                Env.GetString("MOSQUITTO_PASSWORD")
            ).
            WithTlsOptions(o => {
                var clientCertificate = X509CertificateLoader.LoadPkcs12FromFile(
                    Env.GetString("CLIENT_PFX_PATH"),
                    Env.GetString("CLIENT_PFX_PASSWORD")
                );
                o.UseTls(true);
                o.WithClientCertificates(new []{clientCertificate});
                o.WithSslProtocols(SslProtocols.Tls12);
                o.WithAllowUntrustedCertificates(true);
                o.WithCertificateValidationHandler(_ => {
                    return true;
                });
            }).
            Build();

        //device/dispenser/1
        _client.ApplicationMessageReceivedAsync += ApplicationMessageReceivedCallback;
    }
    public async Task StartAsync(CancellationToken cancellationToken){
        await _client.ConnectAsync(_options, cancellationToken);        
        await _client.SubscribeAsync("devices/#");
    }
    public async Task StopAsync(CancellationToken cancellationToken){
        var token = new CancellationTokenSource(TimeSpan.FromMinutes(30)).Token;
        await _client.DisconnectAsync(MqttClientDisconnectOptionsReason.NormalDisconnection,"normal server shutdown",0,null,token);
    }

    private Task ApplicationMessageReceivedCallback(MqttApplicationMessageReceivedEventArgs args){
        var ApplicationMessage = args.ApplicationMessage;
        var topic = ApplicationMessage.Topic;
        var SegmentString = Encoding.UTF8.GetString(ApplicationMessage.Payload);
        Regex regex = new Regex(@"devices/(\w+)/(\d+)");
        Match match = regex.Match(topic);
        string matchDevice = match.Groups[1].Value;
        int matchId = Convert.ToInt32(match.Groups[2].Value);
        string channel = $"{matchDevice}:{matchId}";
        _ = _hub.Clients.Group(channel).SendMessage(SegmentString);
            if(matchDevice == "dispenser" && !ApplicationMessage.Retain){
            _ = _logUpdate.SegmentProcessAsync(matchId, ApplicationMessage.Payload);
        }
        return Task.CompletedTask;
    }
}