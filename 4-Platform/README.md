## 4. Using Platform Features

### Goal
Detect internet access and display a dialog if offline. Implement a custom **IConnectivityService** for both .NET MAUI and Blazor Web app. Create a shared razor component that uses this service to display a message to the user and add it to the **Monkey.razor** page.

### Steps
1. [] **Create the Connectivity Service**

Add `IConnectivityService` and its implementations in both **MyHybridApp** and **MyHybridApp.Web**.

Create the shared interface in **MyHybridApp.Shared** project `\Services\IConnectivityService.cs`:
```csharp
namespace MyHybridApp.Shared.Services;

public interface IConnectivityService
{
    bool IsConnected();
}
```

Create the .NET MAUI implementation in **MyHybridApp** project `\Services\ConnectivityService.cs`:
```csharp
using MyHybridApp.Shared.Services;

namespace MyHybridApp.Services

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
```

Create the Blazor web implementation in **MyHybridApp.Web** project `\Services\ConnectivityService.cs`:
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

2. [] **Register Services**:  

Register `IConnectivityService` in both **MyHybridApp.Web** project's **Program.cs** and **MyHybridApp** project's **MauiProgram.cs**.

```csharp
builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
```

3. [] **Create the Shared Razor Component**

Next add a folder to the Razor Class Library **MyHybridApp.Shared** called `Components` and add a new Razor component called `OfflineAlert.razor`. Add the following code:

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

Now modify the **Pages\Monkeys.razor** to use this component. First inject the `NavigationManager`. We will use this to reload the page after the user clicks **Refresh** on the component. At the top of ths file, under the `@inject IMonkeyService MonkeyService` line, add:

```razor
@inject NavigationManager NavigationManager
```

At the top of the file, under the **@using MyHybridApp.Shared.Models** line add:

```razor
@using MyHybridApp.Shared.Models.Components
```

Then add the component after the `<h1>`:
```razor
<OfflineAlert RefreshCallback="RefreshHandler" />
```

Finally, add the code for the `RefreshHandler` under the `OnInitializedAsync()` method:

```csharp
 async Task RefreshHandler()
 {
     monkeys = await MonkeyService.GetMonkeysAsync(); //try to get the monkeys again
     NavigationManager.NavigateTo("/monkeys", true); //reload the page
 }
```

4. [] **Run the Solution and Disable the Cache**

Build and Debug `F5` the solution. On the Windows app select the Monkey page and notice the Offline component is not displayed. Hit `F12` to start the WebView developer tools. 

Open the Network tab and select **Disable cache** so that the pictures of the monkeys won't be loaded from disk but be requested from the internet every time. 

![](/images/WebDevTools.jpg)

>**Tip:** If the WebView has focus, you can use the `F12` Web Developer tools to debug the rendered web app (html, css, javascript) inside the WebView.

5. [] **Go offline and Test the ConnectivityService**

To simulate going offline on Windows, open the Device Manager and select Network Adapters. Right-click and **Disable device** the Ethernet Adapter. 

On the Windows app, navigate back to the Mokey page and you will see the **OfflineAlert.razor** component displayed. 

![](/images/OfflineAlert.jpg)

In the Device Manager, right-click and **Enable device** on the Enternet Adapter. On the Windows app, click the **Refresh** button and the page will reload. 

### Check-in

At the end of this section, you should have an app with all the functionality working and running as provided in the [4-Platform](../4-Platform/) folder.