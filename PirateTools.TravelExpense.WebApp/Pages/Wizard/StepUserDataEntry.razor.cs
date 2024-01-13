using PirateTools.TravelExpense.WebApp.Models;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserDataEntry {
    private Guid SelectedFedeationId {
        get => AppData.CurrentReport?.Pirate?.Federation?.Id ?? Guid.Empty;
        set {
            if (AppData.CurrentReport == null || AppData.CurrentReport.Pirate == null)
                return;

            AppData.CurrentReport.Pirate.Federation = AppData.Federations.Find(f => f.Id == value)!;
        }
    }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.Pirate == null) {
            AppData.CurrentReport.Pirate = new();
            AppData.CurrentReport.Pirate.Address = new();
        }

        if (AppData.CurrentReport.Pirate.Federation == null)
            AppData.CurrentReport.Pirate.Federation = AppData.Federations[0];

        AppData.CurrentReport.UsedExistingUser = false;
        AppData.CurrentStep = WizardStep.UserDataEntry;
    }
}