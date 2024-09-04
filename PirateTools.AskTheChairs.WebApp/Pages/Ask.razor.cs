using Microsoft.AspNetCore.Components;
using PirateTools.AskTheChairs.WebApp.Services;
using PirateTools.Models.AskYourChairs;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PirateTools.AskTheChairs.WebApp.Pages;

public partial class Ask {
    private string Title = "";
    private string Content = "";
    private string EMail = "";

    private bool Submitting;
    private string? SubmitResult;

    private bool CanSubmit
        => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Content) && !Submitting;

    [Inject]
    public required BackendService BackendService { get; set; }
    [Inject]
    public required AppStateService AppStateService { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private async Task AskQuestion() {
        var question = new Question {
            Title = Title,
            Content = Content,
            EMail = EMail
        };

        Submitting = true;
        var response = await BackendService.ApiClient.PostAsJsonAsync(
            "AskYourChairs/AskQuestion", question);
        Submitting = false;

        if (response.IsSuccessStatusCode) {
            NavigationManager.NavigateTo("/");
            return;
        }

        SubmitResult = "Es ist ein Fehler beim erstellen deiner Frage aufgetreten. Bitte versuche es später nochmal.";
    }
}