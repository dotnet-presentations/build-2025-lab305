using MyHybridApp.Shared.Services;

namespace MyHybridApp.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
