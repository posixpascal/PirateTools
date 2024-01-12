using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using BlazorDownloadFile;
using PirateTools.TravelExpense.WebApp.Services;
using PdfSharpCore.Fonts;
using PirateTools.TravelExpense.WebApp.PDF;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages;

public partial class Home {
    private bool Loading = true;

    [Inject]
    public required IStorageManagerService StorageManager { get; set; }

    [Inject]
    public required HttpClient Http { get; set; }

    [Inject]
    public required IBlazorDownloadFileService DownloadFileService { get; set; }

    [Inject]
    public required FontService FontService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            GlobalFontSettings.FontResolver = new CustomFontResolver(FontService);

            await FontService.LoadFontAsync("OpenSans-Regular.ttf");
            await FontService.LoadFontAsync("OpenSans-Bold.ttf");
            await FontService.LoadFontAsync("OpenSans-Italic.ttf");
            await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");
        }
    }

    private async Task TestFillAsync() {
        var data = await Http.GetStreamAsync("Resources/TravelExpensePDFs/LVNDS_LO.pdf");
        var pdf = PdfReader.Open(data);
        var form = pdf.AcroForm;

        Console.WriteLine(string.Join(',', form.Fields.Names));
        form.Fields["Name"].Value = new PdfString("Test", PdfStringEncoding.Unicode);

        using var memoryStream = new MemoryStream();
        pdf.Save(memoryStream);
        await DownloadFileService.DownloadFile("test.pdf", memoryStream, "application/octet-stream");
    }
}