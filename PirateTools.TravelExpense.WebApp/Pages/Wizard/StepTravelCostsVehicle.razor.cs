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

        AppData.CurrentStep = WizardStep.TravelCostsVehicle;
        AppData.CurrentReport.VehicleUsed = VehicleUsed;
    }

    private double GetFactor() => VehicleUsed == Vehicle.Car ? 0.3 : 0.13;
}