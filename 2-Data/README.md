## 2. Fetching Data

### Goal
Create a service that fetches a list of monkeys for use in our app. The data will reside on the Blazor server app and we'll expose a web API to the .NET MAUI client. 

### Steps
1. **Create the Monkey shared data model**

In this step, first create the data model. Right-click on the `MyHybridApp.Shared` project and **Add** -> **New Folder** name is `Models`. Right-click on this folder and **Add** -> **Class** and name it `Monkey.cs`. Insert the following code:
```csharp
namespace MyHybridApp.Shared.Models
{
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
}
```

2. **Create the MonkeyService Interface**

In this step, you will create a new interface called `IMonkeyService` in the `MyHybridApp.Shared` project. The interface will include methods for retrieving all monkeys and fetching a specific monkey by its name. In the `Services` folder, add a new `IMonkeyService.cs` with the following code:
```csharp
using MyHybridApp.Shared.Models;

namespace MyHybridApp.Shared.Services
{
    public interface IMonkeyService
    {
        Task<List<Monkey>> GetMonkeysAsync();
        Monkey? GetMonkeyByName(string name);
    }
}
```
3. **Create the MonkeyService Server Implementation**

Next, we will define the `MonkeyService` implementations. 
The service will have a list of Monkey data that is returned to the shared UI. If we're on the web server, this will just return a list of Monkeys. If we're on the .NET MAUI client, we will make an `HttpClient` request to get the data. This service will handle returning the data and caching it in memory to avoid redundant API calls. For simplicity, the data is hard coded, but typically this would come from a database or another web service. 

Add a new class `Services\MonkeyService.cs` to the `MyHybridApp.Web` project with this code:

```csharp
using MyHybridApp.Shared.Models;
using MyHybridApp.Shared.Services;

namespace MyHybridApp.Web.Services
{
    public class MonkeyService : IMonkeyService
    {
        public async Task<List<Monkey>> GetMonkeysAsync()
        {            
            return await Task.FromResult(_monkeys);
        }

        public Monkey? GetMonkeyByName(string name)
        {
           return _monkeys?.FirstOrDefault(m => m.Name == name);
        }

        private List<Monkey> _monkeys =
        [
            new Monkey { Name = "Baboon", Location = "Africa & Asia", Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/baboon.jpg", Population = 10000, Latitude = -8.783195, Longitude =  34.508523 },
            new Monkey { Name = "Capuchin Monkey", Location = "Central & South America", Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/capuchin.jpg", Population = 23000, Latitude = 12.769013, Longitude = -85.602364 },
            new Monkey { Name = "Blue Monkey", Location = "Central and East Africa", Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/bluemonkey.jpg", Population = 12000, Latitude = 1.957709, Longitude = 37.297204 },
            new Monkey { Name = "Squirrel Monkey", Location = "Central & South America", Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/saimiri.jpg", Population = 11000, Latitude = -8.783195, Longitude = -55.491477 },
            new Monkey { Name = "Golden Lion Tamarin", Location = "Brazil", Details = "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/tamarin.jpg", Population = 19000, Latitude = -14.235004, Longitude = -51.92528 },
            new Monkey { Name = "Howler Monkey", Location = "South America", Details = "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/alouatta.jpg", Population = 8000, Latitude = -8.783195, Longitude = -55.491477 },
            new Monkey { Name = "Japanese Macaque", Location="Japan", Details="The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/macasa.jpg", Population=1000, Latitude=36.204824, Longitude=138.252924 },
            new Monkey { Name = "Mandrill", Location="Southern Cameroon, Gabon, and Congo", Details="The mandrill is a primate of the Old World monkey family, closely related to the baboons and even more closely to the drill. It is found in southern Cameroon, Gabon, Equatorial Guinea, and Congo.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/mandrill.jpg", Population=17000, Latitude=7.369722, Longitude=12.354722 },
            new Monkey { Name = "Proboscis Monkey", Location="Borneo", Details="The proboscis monkey or long-nosed monkey, known as the bekantan in Malay, is a reddish-brown arboreal Old World monkey that is endemic to the south-east Asian island of Borneo.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/borneo.jpg", Population=15000, Latitude=0.961883, Longitude=114.55485 },
            new Monkey { Name = "Sebastian", Location="Seattle", Details="This little trouble maker lives in Seattle with James and loves traveling on adventures with James and tweeting @MotzMonkeys. He by far is an Android fanboy and is getting ready for the new Google Pixel 9!", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/sebastian.jpg", Population=1, Latitude=47.606209, Longitude=-122.332071 },
            new Monkey { Name = "Henry", Location="Phoenix", Details="An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. His favorite platform is iOS by far and is excited for the new iPhone Xs!", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/henry.jpg", Population=1, Latitude=33.448377, Longitude=-112.074037 },
            new Monkey { Name = "Red-shanked douc", Location="Vietnam", Details="The red-shanked douc is a species of Old World monkey, among the most colourful of all primates. The douc is an arboreal and diurnal monkey that eats and sleeps in the trees of the forest.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg", Population=1300, Latitude=16.111648, Longitude=108.262122 },
            new Monkey { Name = "Mooch", Location="Seattle", Details="An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. Her favorite platform is iOS by far and is excited for the new iPhone 16!", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Mooch.PNG", Population=1, Latitude=47.608013, Longitude=-122.335167 }
        ];        
    }
}
```
4. **Register the Service and Expose the Endpoint**

 To make the `MonkeyService` available throughout the app, you need to register it in the dependency injection container in the `Program.cs`. Add this service as a scoped service. Scoped services are created once per request. They are useful for services that need to maintain data consistency during an operation and are tied to a specific request. Add this code under the registration of the `FormFactor` service:

```csharp
builder.Services.AddScoped<IMonkeyService, MonkeyService>();
```
Next, expose a minimal web api so remote clients can fetch the monkey data from the server. Add this code right before the last line `app.Run()` code:
```csharp
//Expose an endpoint to our MAUI client to get the monkey data
app.MapGet("/api/monkeys", async (IMonkeyService monkeyService) =>
{
    var monkeys = await monkeyService.GetMonkeysAsync();
    return Results.Ok(monkeys);
});
```

5. **Create the MonkeyService Client Implementation**

In order to call the service locally, we'll need the Url that is configured for development. In the `MyHybridApp.Web` project open the `Properties\launchSettings.json` file and make note of the applicationUrl for the `https` profile:

```json
  "https": {
   "commandName": "Project",
   "dotnetRunMessages": true,
   "launchBrowser": true,
   "applicationUrl": "https://localhost:7252;http://localhost:5190",
   "environmentVariables": {
     "ASPNETCORE_ENVIRONMENT": "Development"
   }
 },
```
In this example, the Url is `https://localhost:7252`. This will be the base address we will use to call the web server from our .NET MAUI client. Add a new class `Services\MonkeyService.cs` to the `MyHybridApp` MAUI project. Add the following code, replacing the base address in `_monkeyUri` with the Url:

```csharp
using MyHybridApp.Shared.Models;
using MyHybridApp.Shared.Services;
using System.Diagnostics;
using System.Net.Http.Json;

namespace MyHybridApp.Services
{
    public class MonkeyService : IMonkeyService
    {
        private List<Monkey> _cachedMonkeys = new List<Monkey>();
        private readonly HttpClient _httpClient = new HttpClient();
        
        //replace port # with one from launchSettings.json
        private string _monkeyUri = "https://localhost:7252/api/monkeys"; 
              
        public async Task<List<Monkey>> GetMonkeysAsync() 
        {
            if (_cachedMonkeys.Count > 0)
                return _cachedMonkeys;

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Monkey>>(_monkeyUri);
                _cachedMonkeys = response ?? new List<Monkey>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching data: {ex.Message}");    
            }    
            
            return _cachedMonkeys;
        }

        public Monkey? GetMonkeyByName(string name)
        {
            return _cachedMonkeys?.FirstOrDefault(m => m.Name == name);
        }
    }
}
```
Finally, open the `MauiProgram.cs` and register the service. Put this line under the `FormFactor` registration:

```csharp
builder.Services.AddScoped<IMonkeyService, MonkeyService>();
```

>**Important Note:** Typically you would not hard code the Url of an endpoint in the client, but use a configuration file saved to the devices' storage. In .NET MAUI you can use [`Microsoft.Maui.Storage.Preferences`](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?view=net-maui-9.0&tabs=windows) and/or [`Microsoft.Maui.Storage.SecureStorage`](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/secure-storage?view=net-maui-9.0&tabs=windows).

>**Note on Android Emulators & iOS Simulators**: This lab can only run Windows and Web apps, however, if you wanted to debug to emulators and simulators you can use the [`HttpClientHelper`](https://github.com/dotnet-presentations/build-2025-lab305/blob/f7075ad8cca2306ae1f90a3439ca83a29c022e0e/Finish/MyHybridApp/MyHybridApp/Services/HttpClientHelper.cs#L6) defined in the [`Finish`](../Finish/) folder which configures the http requests so they work correctly. 

### Check-in

Rebuild the solution and make sure there are no errors. At the end of this section, your code should match the solution provided in the [2-Data](../2-Data/) folder.
