using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserSelection {
    protected override void OnAfterRender(bool firstRender) {
        AppData.CurrentStep = WizardStep.UserSelection;
    }
}