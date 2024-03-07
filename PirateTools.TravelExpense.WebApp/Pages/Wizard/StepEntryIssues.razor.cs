using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepEntryIssues {
    protected override void OnParametersSet() {
        //AppData.CurrentStep = WizardStep.ExpenseBaseDataEntry;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        //if (AppData.CurrentReport.IsDonatingAll()) {
            NavigationManager.NavigateTo("StepDonationSelection");
        //} else {
        //    NavigationManager.NavigateTo("StepBankInformationEntry");
        //}
    }

    private string ClassForSeverity(ReportIssueSeverity severity) {
        return severity switch {
            ReportIssueSeverity.Warning => "warning",
            ReportIssueSeverity.Error => "danger",
            _ => ""
        };
    }

    private string IconForSeverity(ReportIssueSeverity severity) {
        return severity switch {
            ReportIssueSeverity.Warning => "bi bi-exclamation-circle-fill",
            ReportIssueSeverity.Error => "bi bi-exclamation-octagon-fill",
            _ => ""
        };
    }
}