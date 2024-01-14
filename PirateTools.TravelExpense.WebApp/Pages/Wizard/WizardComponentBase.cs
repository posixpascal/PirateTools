using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.TravelExpense.WebApp.Utility;
using System.IO;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public class WizardComponentBase : ComponentBase {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    [Inject]
    public required IStorageManagerService StorageManager { get; set; }

    protected override async Task OnParametersSetAsync() {
        if (AppData.CurrentReport == null) {
            NavigationManager.NavigateTo("");
            return;
        }

        await AppData.SaveReports();
    }

    protected async Task SaveImage(string filename, Stream fileStream) {
        var opfsHandle = await StorageManager.GetOriginPrivateDirectoryAsync();
        var imgDirHandle = await opfsHandle.GetDirectoryHandleAsync("images", StorageUtility.DefaultDirOptions);

        await imgDirHandle.StoreFile(filename, fileStream);
    }

    protected async Task AllowLocalStorage() {
        AppData.Config.UseLocalStorage = true;
        await AppData.SaveConfigAsync();
    }
}