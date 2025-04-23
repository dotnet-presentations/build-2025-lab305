using MyHybridApp.Shared.Services;

namespace MyHybridApp.Web.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected()
    {
        return true; // Always return true for web
    }
}