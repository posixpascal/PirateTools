using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelCostsPublicTransit {
    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        AppData.CurrentStep = WizardStep.TravelCostsPublicTransit;
        AppData.CurrentReport.VehicleUsed = Vehicle.PublicTransit;
    }
}