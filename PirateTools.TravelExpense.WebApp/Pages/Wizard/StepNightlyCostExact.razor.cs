using PirateTools.Models.Legacy;
using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepNightlyCostExact {
    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        //if (AppData.CurrentReport.ImageAccommodationReceipt.Count == 0)
        //    AppData.CurrentReport.ImageAccommodationReceipt.Add(new());

        //AppData.CurrentStep = WizardStep.NightlyCostTypeSelection;
    }

    private void AddEntry() { } // => AppData.CurrentReport?.ImageAccommodationReceipt.Add(new());

    private void OnRemoveClicked(TravelExpenseReport_V0.ImageReference imgRef) {
        if (AppData.CurrentReport == null)
            return;

        //AppData.CurrentReport.ImageAccommodationReceipt.Remove(imgRef);

        //if (AppData.CurrentReport.ImageAccommodationReceipt.Count == 0)
        //    AppData.CurrentReport.ImageAccommodationReceipt.Add(new());
    }
}