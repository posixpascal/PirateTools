using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsOverview {
    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.OtherCosts.Count == 0)
            AppData.CurrentReport.OtherCosts.Add(new());

        AppData.CurrentStep = WizardStep.OtherCostsOverview;
    }

    private void AddNew() => AppData.CurrentReport!.OtherCosts.Add(new());

    private void Delete(int index) => AppData.CurrentReport!.OtherCosts.RemoveAt(index);
}