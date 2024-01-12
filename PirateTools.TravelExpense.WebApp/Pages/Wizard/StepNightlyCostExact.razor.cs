using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepNightlyCostExact {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        AppData.CurrentStep = WizardStep.NightlyCostTypeSelection;
    }
}