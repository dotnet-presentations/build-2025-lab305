using MyHybridApp.Shared.Services;
using MyHybridApp.Web.Components;
using MyHybridApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add device-specific services used by the MyHybridApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped<IMonkeyService, MonkeyService>();
builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MyHybridApp.Shared._Imports).Assembly);

//Expose an endpoint to our MAUI client to get the monkey data
app.MapGet("/api/monkeys", async (IMonkeyService monkeyService) =>
{
    var monkeys = await monkeyService.GetMonkeysAsync();
    return Results.Ok(monkeys);
});

app.Run();
