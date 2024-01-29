using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepEntryIssues {
    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.ExpenseBaseDataEntry;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.IsDonatingAll()) {
            NavigationManager.NavigateTo("StepDonationSelection");
        } else {
            NavigationManager.NavigateTo("StepBankInformationEntry");
        }
    }
}