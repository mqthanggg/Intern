public class MqttService : IMqttService{
    private class WSDispenserRecord{
        public float liter {get; set;}
        public int price {get; set;}
        public DispenserState state{get; set;}
        public int payment {get; set;}
    }
    public ConcurrentDictionary<string, List<WebSocket>> connections;
    private readonly ConcurrentDictionary<string, ArraySegment<byte>> _retainMessages;
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;
    private readonly ILogUpdateService _logUpdate;
    public MqttService(
        ILogUpdateService logUpdate,
        IConfiguration configuration
    ){
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

        connections = new ConcurrentDictionary<string, List<WebSocket>>();
        _retainMessages = new ConcurrentDictionary<string, ArraySegment<byte>>();
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
        foreach (var kvp in connections)
        {
            var sockets = kvp.Value;
            lock(sockets){
                sockets.RemoveAll(e => e.State != WebSocketState.Open);
            }
            foreach (var socket in sockets)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Server shut down",
                    token
                );
            }
        }
    }

    public void AddSocket(string channel, WebSocket webSocket){
        connections.AddOrUpdate(channel, [webSocket],(key, list) => {
            lock(list){
                list.Add(webSocket);
            }
            return list;
        });
        if (_retainMessages.TryGetValue(channel,out var _)){
            _ = webSocket.SendAsync(
                _retainMessages[channel],
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
    public void RemoveSocket(string channel, WebSocket webSocket){
        var list = connections[channel];
        lock(list){
            list.Remove(webSocket);
        }
    }

    private Task ApplicationMessageReceivedCallback(MqttApplicationMessageReceivedEventArgs args){
        var ApplicationMessage = args.ApplicationMessage;
        var topic = ApplicationMessage.Topic;
        var segment = new ArraySegment<byte>(ApplicationMessage.Payload.ToArray());
        Regex regex = new Regex(@"devices/(\w+)/(\d+)");
        Match match = regex.Match(topic);
        string matchDevice = match.Groups[1].Value;
        int matchId = Convert.ToInt32(match.Groups[2].Value);
        string channel = $"{matchDevice}:{matchId}";
        if (ApplicationMessage.Retain)
            _retainMessages[channel] = segment;
        else if(matchDevice == "dispenser"){
            _ = _logUpdate.SegmentProcessAsync(matchId, ApplicationMessage.Payload);
        }
        foreach(var kvp in connections){
            string device = kvp.Key.Split(":")[0];
            int id = Convert.ToInt32(kvp.Key.Split(":")[1]);
            List<WebSocket> sockets = kvp.Value;
            if (device == matchDevice && id == matchId){
                lock(sockets){
                    sockets.RemoveAll(e => e.State != WebSocketState.Open);
                    foreach(var socket in sockets){
                        _ = socket.SendAsync(
                            segment,
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None
                        );
                    }
                }
            }
        }
        return Task.CompletedTask;
    }
}