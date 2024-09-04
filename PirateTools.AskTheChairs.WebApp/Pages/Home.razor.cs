using Microsoft.AspNetCore.Components;
using PirateTools.AskTheChairs.WebApp.Models;
using PirateTools.AskTheChairs.WebApp.Services;
using PirateTools.Models.AskYourChairs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PirateTools.AskTheChairs.WebApp.Pages;

public partial class Home {
    [Inject]
    public required AppStateService AppState { get; set; }
    [Inject]
    public required BackendService BackendService { get; set; }
    [Inject]
    public required HttpClient Http { get; set; }

    private Question? OpenQuestion;

    protected override async Task OnParametersSetAsync() {
        var questions = await BackendService.ApiClient!.GetFromJsonAsync<List<Question>>(
            "AskYourChairs/GetQuestions");

        if (questions != null) {
            AppState.Questions = questions;
            StateHasChanged();
        }
    }

    private void Open(Question question) {
        if (OpenQuestion == question) {
            OpenQuestion = null;
            return;
        }

        OpenQuestion = question;
    }
}