using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserSelection {
    protected override void OnAfterRender(bool firstRender) {
        AppData.CurrentStep = WizardStep.UserSelection;
    }

    private void GoToUserSelector() {
        if (AppData.CurrentReport == null || AppData.Config.Users.Count == 0)
            return;

        NavigationManager.NavigateTo("/StepUserSelector");
    }
}