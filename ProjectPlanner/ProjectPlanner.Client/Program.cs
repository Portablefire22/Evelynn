using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ProjectPlanner.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        
        builder.Services.AddScoped<HttpClient>(sp =>
        {
            // Use the server's base address for API calls in Server render mode
            var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
            return new HttpClient
            {
                BaseAddress = new Uri(navigationManager.BaseUri) // Server's BaseUri
            };
        });
        
        
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();

        await builder.Build().RunAsync();
    }
}