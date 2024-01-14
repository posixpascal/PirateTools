using BlazorDownloadFile;
using Microsoft.AspNetCore.Components;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.PDF;
using PirateTools.TravelExpense.WebApp.Services;
using System;
using System.Globalization;
using System.IO;
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

        await FontService.LoadFontAsync("OpenSans-Regular.ttf");
        await FontService.LoadFontAsync("OpenSans-Bold.ttf");
        await FontService.LoadFontAsync("OpenSans-Italic.ttf");
        await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");

        await PdfBuilder.BuildPdfAsync(AppData.CurrentReport, FontService, Http, Culture, DownloadFileService, StorageManager);
    }
}