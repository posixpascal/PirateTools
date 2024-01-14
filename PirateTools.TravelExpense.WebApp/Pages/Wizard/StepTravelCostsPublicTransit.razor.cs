using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelCostsPublicTransit {
    private bool SavingImage;

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        AppData.CurrentStep = WizardStep.TravelCostsPublicTransit;
        AppData.CurrentReport.VehicleUsed = Vehicle.PublicTransit;
    }

    private async Task OnFileChanged(InputFileChangeEventArgs e) {
        if (AppData.CurrentReport == null)
            return;

        SavingImage = true;
        await SaveImage(e.File.Name, e.File.OpenReadStream(4_096_000));
        AppData.CurrentReport.ImagePublicTransitReceipt = e.File.Name;
        SavingImage = false;
    }
}