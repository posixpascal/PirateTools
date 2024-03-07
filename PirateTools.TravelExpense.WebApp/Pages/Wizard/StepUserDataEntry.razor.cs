using PirateTools.TravelExpense.WebApp.Models;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserDataEntry {
    private Guid SelectedFedeationId {
        get => AppData.CurrentReport?.User?.Federation?.Id ?? Guid.Empty;
        set {
            if (AppData.CurrentReport == null || AppData.CurrentReport.User == null)
                return;

            AppData.CurrentReport.User.Federation = AppData.Federations.Find(f => f.Id == value)!;
        }
    }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.User == null) {
            AppData.CurrentReport.User = new();
            AppData.CurrentReport.User.Address = new();
        }

        if (AppData.CurrentReport.User.Federation == null)
            AppData.CurrentReport.User.Federation = AppData.Federations[0];

        //AppData.CurrentReport.UsedExistingUser = false;
        //AppData.CurrentStep = WizardStep.UserDataEntry;
    }
}