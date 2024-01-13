using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System.Globalization;

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
            NavigationManager.NavigateTo("/StepSummary");
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