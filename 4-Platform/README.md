## 4. Using Platform Features

### Goal
Detect internet access and display a dialog if offline. Implement a custom **IConnectivityService** for both .NET MAUI and Blazor Web app. Create a shared Razor component that uses this service to display a message to the user and add it to the **Monkeys.razor** page.

### Steps
1. [] **Create the Connectivity Service Interface**

Create the shared interface in the **MyHybridApp.Shared** project **\Services\IConnectivityService.cs**:
```csharp
namespace MyHybridApp.Shared.Services;

public interface IConnectivityService
{
    bool IsConnected();
}
```

2. [] **Create the Connectivity Service for .NET MAUI**

Create the .NET MAUI implementation in the **MyHybridApp** project **\Services\ConnectivityService.cs**:

```csharp
using MyHybridApp.Shared.Services;

namespace MyHybridApp.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
```

3. [] **Create the Connectivity Service for Web**

Create the Blazor web implementation in **MyHybridApp.Web** project **\Services\ConnectivityService.cs**:
```csharp
using MyHybridApp.Shared.Services;

namespace MyHybridApp.Web.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected()
    {
        return true; // Always return true for web
    }
}
```

4. [] **Register Services**:  

Register `IConnectivityService` in both **MyHybridApp.Web** project's **Program.cs** and **MyHybridApp** project's **MauiProgram.cs**.

```csharp
builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
```

5. [] **Create the Shared Razor Component**

Next add a folder to the Razor Class Library **MyHybridApp.Shared** called **Components** and add a new Razor component called **OfflineAlert.razor**. Add the following code:

```razor
@using MyHybridApp.Shared.Services  
@inject IConnectivityService ConnectivityService

<div class="alert alert-danger" style="display: @(ConnectivityService.IsConnected() ? "none" : "block")">
    <p>No internet connection. Please check your connection and try again.</p>
    <button class="btn btn-primary" @onclick="Refresh">Refresh</button>
</div>

@code {
    [Parameter] public EventCallback<string> RefreshCallback { get; set; }
   
    void Refresh()
    {
        RefreshCallback.InvokeAsync(string.Empty); 
    }
}
```

6. [] **Use Shared Razor Component**

Now modify the **Pages\Monkeys.razor** to use this component. First, inject the `NavigationManager`. We will use this to reload the page after the user clicks **Refresh** on the component. At the top of this file, under the `@inject IMonkeyService MonkeyService` line, add:

```razor
@inject NavigationManager NavigationManager
```

At the top of the file, under the `@using MyHybridApp.Shared.Models` line add:

```razor
@using MyHybridApp.Shared.Components
```

Then add the component after the `<h1>`:
```razor
<OfflineAlert RefreshCallback="RefreshHandler" />
```

Finally, add the code for the `RefreshHandler` under the `OnInitializedAsync()` method:

```csharp
async Task RefreshHandler()
{
    monkeys = await MonkeyService.GetMonkeysAsync(); // Try to get the monkeys again
    NavigationManager.NavigateTo("/monkeys", true); // Reload the page
}
```

7. [] **Run the Solution and Disable the Cache**

Build and Debug `F5` the solution. On the **Windows** app, select the **Monkeys** page and notice the Offline component is not displayed. Hit `F12` to start the WebView developer tools. 

Open the Network tab and select **Disable cache** so that the pictures of the monkeys won't be loaded from disk but be requested from the internet every time. 

![](./../images/WebDevTools.jpg)

>**Tip:** If the WebView has focus and you are running in `DEBUG` mode, you can use the `F12` Web Developer tools to debug the rendered web app (HTML, CSS, JavaScript) inside the WebView. For more information, see [the documentation](https://learn.microsoft.com/aspnet/core/blazor/hybrid/developer-tools?view=aspnetcore-9.0&viewFallbackFrom=net-maui-9.0&pivots=android&toc=%2Fdotnet%2Fmaui%2Ftoc.json&bc=%2Fdotnet%2Fmaui%2Fbreadcrumb%2Ftoc.json).

8. [] **Go offline and Test the ConnectivityService**

To simulate going offline on Windows, open the **Device Manager** in Windows from the start menu, and select and drop down **Network Adapters**. Right-click on **Microsoft Hyper-V Network Adapter** and **Disable device** for this adapter. 

![](./../images/DeviceManager.png)

On the Windows app, navigate back to the Monkey page and you will see the **OfflineAlert.razor** component displayed. 

![](./../images/OfflineAlert.jpg)

In the **Device Manager**, right-click and **Enable device** on the Ethernet Adapter. On the Windows app, click the **Refresh** button and the page will reload. 

### Check-in

At the end of this section, you should have an app with all the functionality working and running as provided in the [4-Platform](https://github.com/dotnet-presentations/build-2025-lab305/tree/main/4-Platform/) folder.