using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepBankInformationEntry {
    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.BankInformationEntry;
    }
}