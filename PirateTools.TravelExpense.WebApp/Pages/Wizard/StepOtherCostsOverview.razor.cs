using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepOtherCostsOverview {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnParametersSet() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        if (AppData.CurrentReport.OtherCosts.Count == 0)
            AppData.CurrentReport.OtherCosts.Add(new());

        AppData.CurrentStep = WizardStep.OtherCostsOverview;
    }

    private void AddNew() => AppData.CurrentReport!.OtherCosts.Add(new());

    private void Delete(int index) => AppData.CurrentReport!.OtherCosts.RemoveAt(index);
}