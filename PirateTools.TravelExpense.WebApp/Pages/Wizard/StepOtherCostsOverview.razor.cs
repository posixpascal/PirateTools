using Microsoft.AspNetCore.Components.Forms;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsOverview {
    private bool SavingImage;

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.OtherCosts.Count == 0)
            AppData.CurrentReport.OtherCosts.Add(new());

        AppData.CurrentStep = WizardStep.OtherCostsOverview;
    }

    private void AddNew() => AppData.CurrentReport!.OtherCosts.Add(new());

    private void Delete(int index) => AppData.CurrentReport!.OtherCosts.RemoveAt(index);

    private async Task OnFileChanged(TravelExpenseReport.AdditionalCosts entry, InputFileChangeEventArgs e) {
        if (AppData.CurrentReport == null)
            return;

        SavingImage = true;
        await SaveImage(e.File.Name, e.File.OpenReadStream(4_096_000));
        entry.ImageReceipt = e.File.Name;
        SavingImage = false;
    }
}