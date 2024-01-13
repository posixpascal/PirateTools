using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepNightlyCostExact {
    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.NightlyCostTypeSelection;
    }
}