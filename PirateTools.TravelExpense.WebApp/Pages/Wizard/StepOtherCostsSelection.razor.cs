using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsSelection {
    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.OtherCostsSelection;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.AccommodationType == AccommodationType.Specified) {
            NavigationManager.NavigateTo("/StepNightlyCostExact");
        } else {
            NavigationManager.NavigateTo("/StepNightlyCostTypeSelection");
        }
    }
}