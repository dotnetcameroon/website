using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using app.client; // root App component lives in the client project

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// root components (adjust if your App component uses a different namespace)
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register a default HttpClient for the client to call the server API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// add other client-side services here as needed

await builder.Build().RunAsync();
