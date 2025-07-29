public static class Controller{
    public static WebApplication MapController(this WebApplication web){
        web.MapHub<DeviceMonitoringHub>("ws");
        return web;
    }
}