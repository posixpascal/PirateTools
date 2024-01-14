using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorDownloadFile;
using PirateTools.TravelExpense.WebApp.Services;
using System.Globalization;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.PDF;
using Blazored.Modal.Services;
using PirateTools.TravelExpense.WebApp.Components;
using Blazored.Modal;
using PirateTools.TravelExpense.WebApp.Components.Modals;

namespace PirateTools.TravelExpense.WebApp.Pages;

public partial class Home {
    [Inject]
    public required IStorageManagerService StorageManager { get; set; }
    [Inject]
    public required HttpClient Http { get; set; }
    [Inject]
    public required IBlazorDownloadFileService DownloadFileService { get; set; }
    [Inject]
    public required FontService FontService { get; set; }
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    [Inject]
    public required CultureInfo Culture { get; set; }
    [Inject]
    public required IModalService ModalService { get; set; }

    private bool Loading = true;

    protected override async Task OnParametersSetAsync() {
        var modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
            .Add(nameof(SpinnerModal.Title), "Lädt"));
        await AppData.LoadDataAsync();
        modal.Close();
        Loading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            await FontService.LoadFontAsync("OpenSans-Regular.ttf");
            await FontService.LoadFontAsync("OpenSans-Bold.ttf");
            await FontService.LoadFontAsync("OpenSans-Italic.ttf");
            await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");
        }
    }

    private async Task AllowLocalStorageAsync() {
        AppData.Config.UseLocalStorage = true;
        await AppData.SaveConfigAsync();
    }

    private void NewReport() {
        AppData.CurrentReport = new();
        AppData.Reports.Add(AppData.CurrentReport);

        NavigationManager.NavigateTo("/StepUserSelection");
    }

    private async Task DeleteReport(TravelExpenseReport report) {
        AppData.Reports.Remove(report);
        await AppData.SaveReports();
    }

    private void EditReport(TravelExpenseReport report) {
        AppData.CurrentReport = report;
        NavigationManager.NavigateTo("/Overview");
    }

    private async Task BuildPdf(TravelExpenseReport report) {
        var modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
            .Add(nameof(SpinnerModal.Title), "PDF wird erstellt"));
        await FontService.LoadFontAsync("OpenSans-Regular.ttf");
        await FontService.LoadFontAsync("OpenSans-Bold.ttf");
        await FontService.LoadFontAsync("OpenSans-Italic.ttf");
        await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");

        await PdfBuilder.BuildPdfAsync(report, FontService, Http, Culture, DownloadFileService, StorageManager);
        modal.Close();
    }
}