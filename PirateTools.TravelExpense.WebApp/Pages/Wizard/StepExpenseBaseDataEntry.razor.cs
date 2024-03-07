using PirateTools.TravelExpense.WebApp.Models;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepExpenseBaseDataEntry {
    protected override void OnParametersSet() {
        //AppData.CurrentStep = WizardStep.ExpenseBaseDataEntry;

        //AppData.CurrentReport?.FigureOutRegulation();
    }

    private void OnStartDateChanged() { } // => AppData.CurrentReport?.FigureOutRegulation();

    private void OnResolutionIDChanged() {
        if (AppData.CurrentReport == null)
            return;

        if (!string.IsNullOrEmpty(AppData.CurrentReport.ResolutionID) && AppData.CurrentReport.ResolutionDate == null)
            AppData.CurrentReport.ResolutionDate = DateOnly.FromDateTime(DateTime.Now);
    }
}