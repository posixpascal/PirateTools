using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepFederationDataEntry {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private Guid SelectedFedeationId {
        get => AppData.CurrentReport?.Federation?.Id ?? Guid.Empty;
        set {
            if (AppData.CurrentReport == null)
                return;

            AppData.CurrentReport.Federation = AppData.Federations.Find(f => f.Id == value);
        }
    }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        AppData.CurrentStep = WizardStep.FederationDataEntry;
    }
}