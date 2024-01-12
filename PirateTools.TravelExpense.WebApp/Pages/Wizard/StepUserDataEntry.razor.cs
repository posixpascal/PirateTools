using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserDataEntry {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private Guid SelectedFedeationId {
        get => AppData.CurrentReport?.Pirate?.Federation?.Id ?? Guid.Empty;
        set {
            if (AppData.CurrentReport == null || AppData.CurrentReport.Pirate == null)
                return;

            AppData.CurrentReport.Pirate.Federation = AppData.Federations.Find(f => f.Id == value)!;
        }
    }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        if (AppData.CurrentReport.Pirate == null) {
            AppData.CurrentReport.Pirate = new();
            AppData.CurrentReport.Pirate.Address = new();
        }

        AppData.CurrentStep = WizardStep.UserDataEntry;
    }
}