using Microsoft.AspNetCore.Components;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System.Globalization;
using System.Linq;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepDonationSelection {
    [Inject]
    public required CultureInfo Culture { get; set; }

    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.DonationSelection;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.OtherCosts.Count == 0) {
            NavigationManager.NavigateTo("/StepOtherCostsSelection");
        } else {
            NavigationManager.NavigateTo("/StepOtherCostsOverview");
        }
    }

    private void GoNext() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.IsDonatingAll()) {
            if (TravelExpenseReportValidator.ValidateReport(AppData.CurrentReport).Any()) {
                NavigationManager.NavigateTo("/StepEntryIssues");
            } else {
                NavigationManager.NavigateTo("/StepSummary");
            }
        } else {
            NavigationManager.NavigateTo("/StepBankInformationEntry");
        }
    }

    private bool IsDonateSpecificDisabled() {
        if (AppData.CurrentReport == null)
            return false;

        return AppData.CurrentReport.DonateAll;
    }

    private bool IsDonatePartialDisabled() {
        if (AppData.CurrentReport == null)
            return false;

        return AppData.CurrentReport.DonateAll || AppData.CurrentReport.DonateCustomAmount;
    }
}