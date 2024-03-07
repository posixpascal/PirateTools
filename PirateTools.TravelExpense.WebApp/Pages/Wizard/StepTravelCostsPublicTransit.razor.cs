using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.Models;
using PirateTools.Models.Legacy;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepTravelCostsPublicTransit {
    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        //if (AppData.CurrentReport.ImagePublicTransitReceipt.Count == 0)
        //    AppData.CurrentReport.ImagePublicTransitReceipt.Add(new());

        //AppData.CurrentStep = WizardStep.TravelCostsPublicTransit;
        //AppData.CurrentReport.VehicleUsed = Vehicle.PublicTransit;
    }

    private void AddEntry() { } // => AppData.CurrentReport?.ImagePublicTransitReceipt.Add(new());

    private void OnRemoveClicked(TravelExpenseReport_V0.ImageReference imgRef) {
        if (AppData.CurrentReport == null)
            return;

        //AppData.CurrentReport.ImagePublicTransitReceipt.Remove(imgRef);

        //if (AppData.CurrentReport.ImagePublicTransitReceipt.Count == 0)
        //    AppData.CurrentReport.ImagePublicTransitReceipt.Add(new());
    }
}