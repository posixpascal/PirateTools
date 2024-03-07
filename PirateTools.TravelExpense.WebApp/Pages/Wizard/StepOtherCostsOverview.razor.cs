using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.Models.Legacy;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsOverview {
    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        //if (AppData.CurrentReport.OtherCosts.Count == 0) {
        //    if (AppData.CurrentReport.Regulation.OtherCostTemplates.Count != 0) {
        //        foreach (var template in AppData.CurrentReport.Regulation.OtherCostTemplates) {
        //            AppData.CurrentReport.OtherCosts.Add(new TravelExpenseReport_V0.AdditionalCosts { Text = template });
        //        }
        //    } else {
        //        AppData.CurrentReport.OtherCosts.Add(new());
        //    }
        //}

        //AppData.CurrentStep = WizardStep.OtherCostsOverview;
    }

    private void AddNew() { } // => AppData.CurrentReport!.OtherCosts.Add(new());

    private void Delete(int index) { } // => AppData.CurrentReport!.OtherCosts.RemoveAt(index);
}