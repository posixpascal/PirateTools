using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PirateTools.AskTheChairs.WebApp.Models;
using PirateTools.AskTheChairs.WebApp.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PirateTools.AskTheChairs.WebApp;

public static class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        builder.Services.AddSingleton(http);

        var config = await http.GetFromJsonAsync<Config>("config.json");
        builder.Services.AddSingleton(new BackendService(config?.ApiUrl ?? ""));

        builder.Services.AddSingleton<AppStateService>();

        await builder.Build().RunAsync();
    }
}