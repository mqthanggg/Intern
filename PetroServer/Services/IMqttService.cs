using System.Net.WebSockets;

public interface IMqttService : IHostedService{
    void AddSocket(string channel, WebSocket webSocket);
    void RemoveSocket(string channel, WebSocket webSocket);
}