using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelCostsVehicle {
    [Inject]
    public required CultureInfo Culture { get; set; }
    [Parameter]
    public required int VehicleUsedInt { get; set; }

    private bool SavingImage;

    private Vehicle VehicleUsed => (Vehicle)VehicleUsedInt;

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        AppData.CurrentStep = WizardStep.TravelCostsVehicle;
        AppData.CurrentReport.VehicleUsed = VehicleUsed;
    }

    private double GetFactor() => VehicleUsed == Vehicle.Car ? 0.3 : 0.13;

    private async Task OnFileChanged(InputFileChangeEventArgs e) {
        if (AppData.CurrentReport == null)
            return;

        SavingImage = true;
        await SaveImage(e.File.Name, e.File.OpenReadStream(4_096_000));
        AppData.CurrentReport.ImageMapRoute = e.File.Name;
        SavingImage = false;
    }
}