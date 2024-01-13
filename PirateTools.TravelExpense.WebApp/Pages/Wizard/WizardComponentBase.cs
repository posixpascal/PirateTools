using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Services;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public class WizardComponentBase : ComponentBase {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnParametersSetAsync() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        await AppData.SaveReports();
    }
}