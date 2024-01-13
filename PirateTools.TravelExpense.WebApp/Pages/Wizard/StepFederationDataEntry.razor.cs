using PirateTools.TravelExpense.WebApp.Models;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepFederationDataEntry {
    private Guid SelectedFedeationId {
        get => AppData.CurrentReport?.Federation?.Id ?? Guid.Empty;
        set {
            if (AppData.CurrentReport == null)
                return;

            AppData.CurrentReport.Federation = AppData.Federations.Find(f => f.Id == value);
        }
    }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.Federation == null) {
            if (AppData.CurrentReport.Pirate?.Federation != null) {
                AppData.CurrentReport.Federation = AppData.CurrentReport.Pirate.Federation;
            } else {
                AppData.CurrentReport.Federation = AppData.Federations[0];
            }
        }

        AppData.CurrentStep = WizardStep.FederationDataEntry;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.UsedExistingUser) {
            NavigationManager.NavigateTo("/StepUserSelector");
        } else {
            NavigationManager.NavigateTo("/StepUserDataEntry");
        }
    }
}