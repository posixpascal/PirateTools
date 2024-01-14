using Microsoft.AspNetCore.Components.Forms;
using PirateTools.TravelExpense.WebApp.Models;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepNightlyCostExact {
    private bool SavingImage;

    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.NightlyCostTypeSelection;
    }

    private async Task OnFileChanged(InputFileChangeEventArgs e) {
        if (AppData.CurrentReport == null)
            return;

        SavingImage = true;
        await SaveImage(e.File.Name, e.File.OpenReadStream(4_096_000));
        AppData.CurrentReport.ImageAccommodationReceipt = e.File.Name;
        SavingImage = false;
    }
}