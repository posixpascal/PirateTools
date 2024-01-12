using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelTypeSelection {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        AppData.CurrentStep = WizardStep.TravelTypeSelection;
    }

    private void JumpToNightlyCostsSelection() {
        if (AppData.CurrentReport == null)
            return;

        AppData.CurrentReport.VehicleUsed = Vehicle.Undefined;
        NavigationManager.NavigateTo("/StepNightlyCostTypeSelection");
    }
}