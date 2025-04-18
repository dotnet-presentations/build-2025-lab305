# Building a .NET MAUI Blazor Hybrid App

In this workshop, you'll build a hybrid mobile and desktop application using the .NET MAUI Blazor Hybrid template. This template combines a Blazor Web App and a .NET MAUI app, sharing UI components through a Razor Class Library (RCL). By the end of this 75-minute workshop, you'll have a functional app that displays data, navigates between pages, and uses platform-specific features.

---

## 1. Overview of .NET MAUI and Blazor Hybrid Templates

### Goal
Understand the structure and purpose of the .NET MAUI Blazor Hybrid template. Learn how it enables shared UI components between a Blazor Web App and a .NET MAUI app, and how to create a new project using the template.

### What is .NET MAUI?
.NET Multi-platform App UI (MAUI) is a framework for building cross-platform applications with a single codebase. It supports Android, iOS, macOS, and Windows, enabling developers to create native apps using C# and .NET.

### What is Blazor?
Blazor is a web framework that allows developers to build interactive web applications using C# and Razor syntax. It eliminates the need for JavaScript by enabling developers to write client-side logic in C#. Blazor supports multiple hosting models:

- **Blazor Server**: Runs the application on the server, with UI updates sent to the client over a SignalR connection.
- **Blazor WebAssembly**: Runs the application directly in the browser using WebAssembly, downloading the .NET runtime and app code to the client.
- **Blazor Hybrid**: Embeds Blazor components into native applications using frameworks like .NET MAUI. Unlike other Blazor modes, Blazor Hybrid apps:
  - Do **not** use WebAssembly.
  - Do **not** require a server for hosting.
  - Run .NET logic directly on the device using the .NET runtime provided by .NET MAUI.

### What is Blazor Hybrid?
Blazor Hybrid allows you to embed Blazor components into native apps using .NET MAUI. This approach combines the strengths of web and native development, enabling code reuse and access to native device features. Blazor Hybrid apps leverage the `BlazorWebView` control, which renders Razor components inside a native WebView, while running .NET code directly on the device.

### Creating the Project
1. Open **Visual Studio** and select **Create a new project**.
2. Choose the **.NET MAUI Blazor Hybrid and Web App** template.
3. Configure the project:
   - Set the **Framework** to `.NET 9.0`.
   - Check **Configure for HTTPS**.
   - Set the **Interactive Render Mode** to `Server`.
   - Check **Include sample pages**.
   - **Unselect** the option for "Do not use top-level statements."
4. Name the project `MyHybridApp` and click **Create**.

This will generate a solution with:
- `MyHybridApp.Shared`: Shared Razor components.
- `MyHybridApp.Web`: Blazor Web App.
- `MyHybridApp.Maui`: .NET MAUI Blazor Hybrid App.

---

## 2. Displaying Data

### Goal
Fetch and display a list of monkeys from a JSON API in a Razor component. The main page will show each monkey's name and image, providing a visually appealing list. A new `Monkeys.razor` page will be created for this purpose and added to the navigation menu.

### Steps
1. **Create the MonkeyService**:
   - Add a new file `MonkeyService.cs` in the `MyHybridApp.Shared` project:
     ```csharp
     using System.Net.Http;
     using System.Net.Http.Json;
     using System.Collections.Generic;
     using System.Linq;
     using System.Threading.Tasks;

     public class MonkeyService
     {
         private readonly HttpClient _httpClient;
         private List<Monkey>? _cachedMonkeys;

         public MonkeyService(HttpClient httpClient)
         {
             _httpClient = httpClient;
         }

         public async Task<List<Monkey>> GetMonkeysAsync()
         {
             if (_cachedMonkeys != null)
                 return _cachedMonkeys;

             var response = await _httpClient.GetFromJsonAsync<List<Monkey>>("https://montemagno.com/monkeys.json");
             _cachedMonkeys = response ?? new List<Monkey>();
             return _cachedMonkeys;
         }

         public Monkey? GetMonkeyByName(string name)
         {
             return _cachedMonkeys?.FirstOrDefault(m => m.Name == name);
         }
     }

     public class Monkey
     {        
         public string Name { get; set; } = string.Empty;
         public string Location { get; set; } = string.Empty;
         public string Details { get; set; } = string.Empty;
         public string Image { get; set; } = string.Empty;
         public int Population { get; set; }
         public double Latitude { get; set; }
         public double Longitude { get; set; }
     }
     ```
     **Explanation**:  
     - `MonkeyService` fetches and caches monkey data in memory to avoid redundant API calls.  
     - The `GetMonkeyByName` method retrieves a specific monkey by its name.  
     - The `Monkey` class includes additional properties for detailed information.  
     - Required namespaces like `System.Net.Http` and `System.Net.Http.Json` are included.

2. **Register the MonkeyService**:
   - Open `MauiProgram.cs` in the `MyHybridApp` project and `Program.cs` in the `MyHybridApp.Web` project.
   - Register the `HttpClient` and `MonkeyService`:
     ```csharp
     builder.Services.AddHttpClient<MonkeyService>();
     ```
     **Explanation**:  
     - `HttpClient` is registered with `MonkeyService` to handle HTTP requests.  
     - This ensures the service is available in both the .NET MAUI and Blazor Web App projects.

3. **Create the Monkeys Page**:
   - Add a new Razor component `Monkeys.razor` in the `MyHybridApp.Shared` project:
     ```razor
     @page "/monkeys"
     @inject MonkeyService MonkeyService

     <h1>Monkey Finder</h1>

     @if (monkeys == null)
     {
         <div class="text-center">
             <div class="spinner-border text-primary" role="status">
                 <span class="visually-hidden">Loading...</span>
             </div>
         </div>
     }
     else
     {
         <div class="row">
             @foreach (var monkey in monkeys)
             {
                 <div class="col-md-4">
                     <div class="card">
                         <img src="@monkey.Image" class="card-img-top" alt="@monkey.Name" />
                         <div class="card-body">
                             <h5 class="card-title">@monkey.Name</h5>
                         </div>
                     </div>
                 </div>
             }
         </div>
     }

     @code {
         private List<Monkey>? monkeys;

         protected override async Task OnInitializedAsync()
         {
             monkeys = await MonkeyService.GetMonkeysAsync();
         }
     }
     ```
     **Explanation**:  
     - A loading spinner is displayed while data is being fetched.  
     - The `OnInitializedAsync` lifecycle method fetches the monkey data when the page loads.  
     - Each monkey's name and image are displayed in a Bootstrap card layout.  
     - Navigation links will be added in Part 3.

4. **Add the Monkeys Page to the Navigation Menu**:
   - Open `NavMenu.razor` in the `MyHybridApp.Shared` project.
   - Add a new navigation link for the `Monkeys` page:
     ```razor
     <div class="nav-item px-3">
         <NavLink class="nav-link" href="monkeys">
             <span class="bi bi-list-ul-nav-menu" aria-hidden="true"></span> Monkeys
         </NavLink>
     </div>
     ```
     **Explanation**:  
     - This adds a new menu item labeled "Monkeys" to the navigation menu.  
     - The `href` attribute points to the `/monkeys` route.

---

## 3. Add Navigation

### Goal
Enable navigation to a details page when a monkey is clicked. The details page will display additional information about the selected monkey.

### Steps
1. **Create the Details Page**:
   - Add a new Razor component `DetailsPage.razor` in the `MyHybridApp.Shared` project:
     ```razor
     @page "/details/{name}"
     @inject MonkeyService MonkeyService

     <h1>@monkey?.Name</h1>

     @if (monkey != null)
     {
         <div class="card">
             <img src="@monkey.Image" class="card-img-top" alt="@monkey.Name" />
             <div class="card-body">
                 <h5 class="card-title">@monkey.Name</h5>
                 <p><strong>Location:</strong> @monkey.Location</p>
                 <p><strong>Population:</strong> @monkey.Population</p>
                 <p>@monkey.Details</p>
             </div>
         </div>
     }

     @code {
         [Parameter] public string Name { get; set; } = string.Empty;
         private Monkey? monkey;

         protected override async Task OnParametersSetAsync()
         {
             monkey = await Task.FromResult(MonkeyService.GetMonkeyByName(Name));
         }
     }
     ```
     **Explanation**:  
     - The `@page` directive defines the route for the details page, with a `name` parameter.  
     - The `OnParametersSetAsync` lifecycle method retrieves the monkey details using the `MonkeyService`.  
     - The page displays detailed information about the selected monkey in a card layout.

2. **Update the Monkeys Page for Navigation**:
   - Open `Monkeys.razor` in the `MyHybridApp.Shared` project.
   - Add a navigation link to each monkey card:
     ```razor
     <a href="/details/@monkey.Name" class="stretched-link"></a>
     ```
     **Explanation**:  
     - This adds a clickable link to each monkey card, navigating to the details page for the selected monkey.

3. **Add the Details Page to the Navigation Menu**:
   - Open `NavMenu.razor` in the `MyHybridApp.Shared` project.
   - Add a new navigation link for the `Details` page:
     ```razor
     <div class="nav-item px-3">
         <NavLink class="nav-link" href="details">
             <span class="bi bi-info-circle-nav-menu" aria-hidden="true"></span> Details
         </NavLink>
     </div>
     ```
     **Explanation**:  
     - This adds a new menu item labeled "Details" to the navigation menu.  
     - The `href` attribute points to the `/details` route.

---

## 4. Using Platform Features

### Goal
Detect internet access and display a dialog if offline. Implement a custom `IConnectivityService` and a `DialogService` for both .NET MAUI and Blazor Web App. The web implementation of `DialogService` will use JavaScript to display alerts. Additionally, update the connectivity check to handle fetching the list of monkeys.

### Steps
1. **Create the Connectivity Service**:
   - Add `IConnectivityService` and its implementations in both `MyHybridApp` and `MyHybridApp.Web`.

   **Shared Interface**:
   ```csharp
   public interface IConnectivityService
   {
       bool IsConnected();
   }
   ```

   **.NET MAUI Implementation**:
   ```csharp
   using Microsoft.Maui.Networking;

   public class MauiConnectivityService : IConnectivityService
   {
       public bool IsConnected()
       {
           return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
       }
   }
   ```

   **Blazor Web Implementation**:
   ```csharp
   public class WebConnectivityService : IConnectivityService
   {
       public bool IsConnected()
       {
           return true; // Always return true for web.
       }
   }
   ```

2. **Create the Dialog Service**:
   - Add `IDialogService` and its implementations in both `MyHybridApp` and `MyHybridApp.Web`.

   **Shared Interface**:
   ```csharp
   public interface IDialogService
   {
       Task ShowAlertAsync(string title, string message, string buttonText);
   }
   ```

   **.NET MAUI Implementation**:
   ```csharp
   public class MauiDialogService : IDialogService
   {
       public Task ShowAlertAsync(string title, string message, string buttonText)
       {
           return Application.Current.MainPage.DisplayAlert(title, message, buttonText);
       }
   }
   ```

   **Blazor Web Implementation with JavaScript**:
   ```csharp
   using Microsoft.JSInterop;

   public class WebDialogService : IDialogService
   {
       private readonly IJSRuntime _jsRuntime;

       public WebDialogService(IJSRuntime jsRuntime)
       {
           _jsRuntime = jsRuntime;
       }

       public Task ShowAlertAsync(string title, string message, string buttonText)
       {
           return _jsRuntime.InvokeVoidAsync("alert", $"{title}\n\n{message}").AsTask();
       }
   }
   ```

3. **Register Services**:
   - Register `IConnectivityService` and `IDialogService` in both projects:
     ```csharp
     builder.Services.AddSingleton<IConnectivityService, MauiConnectivityService>();
     builder.Services.AddSingleton<IDialogService, MauiDialogService>();
     ```

     ```csharp
     builder.Services.AddSingleton<IConnectivityService, WebConnectivityService>();
     builder.Services.AddSingleton<IDialogService, WebDialogService>();
     ```

4. **Modify Main Page**:
   - Update `MainPage.razor` to use `IConnectivityService` and `IDialogService`:
     ```razor
     @inject IConnectivityService ConnectivityService
     @inject IDialogService DialogService
     @inject MonkeyService MonkeyService

     <h1>Monkey Finder</h1>

     @if (!isConnected)
     {
         <div class="alert alert-danger">
             <p>No internet connection. Please check your connection and try again.</p>
             <button class="btn btn-primary" @onclick="Refresh">Refresh</button>
         </div>
     }
     else if (monkeys == null)
     {
         <div class="text-center">
             <div class="spinner-border text-primary" role="status">
                 <span class="visually-hidden">Loading...</span>
             </div>
         </div>
     }
     else
     {
         <div class="row">
             @foreach (var monkey in monkeys)
             {
                 <div class="col-md-4">
                     <div class="card">
                         <img src="@monkey.Image" class="card-img-top" alt="@monkey.Name" />
                         <div class="card-body">
                             <h5 class="card-title">@monkey.Name</h5>
                         </div>
                     </div>
                 </div>
             }
         </div>
     }

     @code {
         private List<Monkey>? monkeys;
         private bool isConnected = true;

         protected override async Task OnInitializedAsync()
         {
             await LoadMonkeysAsync();
         }

         private async Task LoadMonkeysAsync()
         {
             isConnected = ConnectivityService.IsConnected();
             if (!isConnected)
             {
                 monkeys = null;
                 await DialogService.ShowAlertAsync("No Itnernet", $"You are disconnected from the internet.", "OK");
                 return;
             }

             try
             {
                 monkeys = await MonkeyService.GetMonkeysAsync();
             }
             catch (Exception ex)
             {
                 await DialogService.ShowAlertAsync("Error", $"Failed to load monkeys: {ex.Message}", "OK");
             }
         }

         private async Task Refresh()
         {
             await LoadMonkeysAsync();
         }
     }
     ```
     **Explanation**:  
     - The `MainPage` now includes a connectivity check before fetching data.  
     - A loading spinner is displayed while data is being fetched.  
     - If there is no internet connection, a "No Internet" message and a refresh button are displayed.  
     - The monkeys are displayed in a consistent card layout.

---

## Conclusion

Congratulations! You've built a hybrid app that displays data, navigates between pages, and uses platform-specific features. Explore further by adding more functionality or styling the app with Bootstrap.
