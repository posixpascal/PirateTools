using BlazorDownloadFile;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Components.Modals;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.PDF;
using PirateTools.TravelExpense.WebApp.Services;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepSummary {
    [Inject]
    public required HttpClient Http { get; set; }
    [Inject]
    public required IBlazorDownloadFileService DownloadFileService { get; set; }
    [Inject]
    public required CultureInfo Culture { get; set; }
    [Inject]
    public required FontService FontService { get; set; }
    [Inject]
    public required IModalService ModalService { get; set; }

    protected override void OnParametersSet() {
        AppData.CurrentStep = WizardStep.Summary;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        if (AppData.CurrentReport.IsDonatingAll()) {
            NavigationManager.NavigateTo("/StepDonationSelection");
        } else {
            NavigationManager.NavigateTo("/StepBankInformationEntry");
        }
    }

    private async Task BuildPDF() {
        if (AppData.CurrentReport == null)
            return;

        var modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
            .Add(nameof(SpinnerModal.Title), "PDF wird erstellt"));
        await FontService.LoadFontAsync("OpenSans-Regular.ttf");
        await FontService.LoadFontAsync("OpenSans-Bold.ttf");
        await FontService.LoadFontAsync("OpenSans-Italic.ttf");
        await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");

        await PdfBuilder.BuildPdfAsync(AppData.CurrentReport, FontService, Http, Culture, DownloadFileService, StorageManager);
        modal.Close();
    }
}