using Blazored.Modal.Services;
using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.TravelExpenseWizard;

public class WizardComponentBase : ComponentBase {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    [Inject]
    public required IStorageManagerService StorageManager { get; set; }
    [Inject]
    public required IModalService ModalService { get; set; }

    protected TravelExpenseReport_V1 Report = default!;

    protected override async Task OnParametersSetAsync() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        await AppData.SaveReports();

        Report = AppData.CurrentReport;
    }
}