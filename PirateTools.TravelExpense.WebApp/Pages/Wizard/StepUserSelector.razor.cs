using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserSelector {
    protected override void OnParametersSet() {
        //AppData.CurrentStep = WizardStep.UserSelector;
    }

    private void UseUser(Pirate user) {
        if (AppData.CurrentReport == null)
            return;

        //AppData.CurrentReport.UsedExistingUser = true;
        AppData.CurrentReport.User = user;
        NavigationManager.NavigateTo("/StepFederationDataEntry");
    }
}