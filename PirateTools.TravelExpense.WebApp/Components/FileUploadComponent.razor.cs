using Blazored.Modal;
using Blazored.Modal.Services;
using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using PirateTools.Models.Legacy;
using PirateTools.TravelExpense.WebApp.Components.Modals;
using PirateTools.TravelExpense.WebApp.Services;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Components;

public partial class FileUploadComponent {
    [Inject]
    public required IModalService ModalService { get; set; }
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required IStorageManagerService StorageManager { get; set; }

    [Parameter]
    public required TravelExpenseReport_V0.ImageReference ImageRef { get; set; }
    [Parameter]
    public required string Title { get; set; }

    [Parameter]
    public long MaxFileSize { get; set; } = 4_194_304;
    [Parameter]
    public EventCallback<TravelExpenseReport_V0.ImageReference> OnRemoveChoiceClicked { get; set; }

    private async Task AllowLocalStorage() {
        AppData.Config.UseLocalStorage = true;
        await AppData.SaveConfigAsync();
    }

    private async Task OnRemoveClicked() {
        ImageRef.FileName = null;
        await OnRemoveChoiceClicked.InvokeAsync(ImageRef);
    }

    private async Task OnOpenModal() {
        var modal = ModalService.Show<FileSelectionModal>("", new ModalParameters()
            .Add(nameof(FileSelectionModal.Title), "Wähle eine Datei")
            .Add(nameof(FileSelectionModal.Path), "images")
            .Add(nameof(FileSelectionModal.MaxFileSize), MaxFileSize));

        var result = await modal.Result;

        if (result.Confirmed) {
            ImageRef.FileName = result.Data?.ToString();
        }
    }
}