using Microsoft.AspNetCore.Components;
using PirateTools.AskTheChairs.WebApp.Services;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PirateTools.AskTheChairs.WebApp.Pages;

public partial class TokenManager {
    [Inject]
    public required AppStateService AppStateService { get; set; }
    [Inject]
    public required BackendService BackendService { get; set; }

    private string EMail = "";

    private bool Submitting;
    private string? RequestTokenResult;
    private string? CheckTokenResult;

    private async Task RequestToken() {
        RequestTokenResult = null;

        Submitting = true;
        var response = await BackendService.ApiClient.PostAsJsonAsync(
            "AskYourChairs/RequestToken", EMail);
        Submitting = false;

        EMail = "";

        if (response.IsSuccessStatusCode) {
            RequestTokenResult = "Ein Token wurde an deine E-Mail gesendet!";
        } else {
            RequestTokenResult = "Es ist ein Fehler beim generieren aufgetreten. Versuche es später nochmal.";
        }
    }

    private async Task CheckToken() {
        CheckTokenResult = null;

        BackendService.ApiClient.DefaultRequestHeaders.Add("AuthToken", AppStateService.Token);

        Submitting = true;
        var response = await BackendService.ApiClient.GetAsync("AskYourChairs/CheckToken");
        Submitting = false;

        if (int.TryParse(await response.Content.ReadAsStringAsync(), out var usagesLeft)) {
            AppStateService.ActionsLeft = usagesLeft;

            if (usagesLeft <= 0) {
                CheckTokenResult = "Der Token hat leider keine Aktionen mehr übrig, bitte generiere einen neuen!";
            } else {
                CheckTokenResult = $"Du kannst mit diesem Token noch {usagesLeft} Aktionen durchführen.";
            }
        } else {
            CheckTokenResult = "Es ist ein Fehler beim prüfen aufgetreten. Versuche es später nochmal.";
        }
    }
}