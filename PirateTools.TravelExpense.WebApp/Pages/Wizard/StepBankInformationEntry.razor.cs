using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System.Linq;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepBankInformationEntry {
    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.BankInformationEntry;
    }

    private void GoNext() {
        if (AppData.CurrentReport == null)
            return;

        if (TravelExpenseReportValidator.ValidateReport(AppData.CurrentReport).Any()) {
            NavigationManager.NavigateTo("/StepEntryIssues");
        } else {
            NavigationManager.NavigateTo("/StepSummary");
        }
    }
}