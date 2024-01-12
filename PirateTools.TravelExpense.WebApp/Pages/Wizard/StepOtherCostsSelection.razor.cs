using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsSelection {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

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