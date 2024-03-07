using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelTypeSelection {
    protected override void OnParametersSet() {
        //AppData.CurrentStep = WizardStep.TravelTypeSelection;
    }

    private void JumpToNightlyCostsSelection() {
        if (AppData.CurrentReport == null)
            return;

        //AppData.CurrentReport.VehicleUsed = Vehicle.Undefined;
        NavigationManager.NavigateTo("/StepNightlyCostTypeSelection");
    }
}