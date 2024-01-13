using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Services;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages;

public partial class Settings {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnParametersSet() {
        if (!AppData.LoadingCompleted) {
            NavigationManager.NavigateTo("/");
            return;
        }
    }

    private async Task KillData() {
        await AppData.DeleteAllData();
    }

    private async Task AllowDataStorage() {
        AppData.Config.UseLocalStorage = true;
        await AppData.SaveConfigAsync();
    }
}