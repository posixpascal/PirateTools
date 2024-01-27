using Microsoft.AspNetCore.Components;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System.Globalization;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelCostsVehicle {
    [Inject]
    public required CultureInfo Culture { get; set; }
    [Parameter]
    public required int VehicleUsedInt { get; set; }

    private Vehicle VehicleUsed => (Vehicle)VehicleUsedInt;

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.ImageMapRoute.Count == 0)
            AppData.CurrentReport.ImageMapRoute.Add(new());

        AppData.CurrentStep = WizardStep.TravelCostsVehicle;
        AppData.CurrentReport.VehicleUsed = VehicleUsed;
    }

    private double GetFactor() => VehicleUsed == Vehicle.Car ? 0.3 : 0.13;

    private void AddEntry() => AppData.CurrentReport?.ImageMapRoute.Add(new());

    private void OnRemoveClicked(TravelExpenseReport.ImageReference imgRef) {
        if (AppData.CurrentReport == null)
            return;

        AppData.CurrentReport.ImageMapRoute.Remove(imgRef);

        if (AppData.CurrentReport.ImageMapRoute.Count == 0)
            AppData.CurrentReport.ImageMapRoute.Add(new());
    }
}