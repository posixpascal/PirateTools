using BlazorDownloadFile;
using KristofferStrube.Blazor.FileSystem;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.TravelExpense.WebApp.Utility;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.PDF;

public static class PdfBuilder {
    private const double A4Height = 842;
    private const double A4Width = 595;

    private static readonly byte[] AttachmentBuffer = new byte[5_000_000];

    public static async Task BuildPdfAsync(TravelExpenseReport report, FontService FontService,
        HttpClient Http, CultureInfo Culture, IBlazorDownloadFileService DownloadFileService,
        IStorageManagerService StorageManager, AppDataService appData, bool readOnly = true) {
        await FontService.LoadFontAsync("OpenSans-Regular.ttf");
        await FontService.LoadFontAsync("OpenSans-Bold.ttf");
        await FontService.LoadFontAsync("OpenSans-Italic.ttf");
        await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");

        report.Federation = appData.Federations.Find(f => f.Id == report.Federation?.Id);
        report.FigureOutRegulation();

        if (report.UsedExistingUser)
            report.Pirate = appData.Config.Users.Find(p => p.Id == report.Pirate!.Id);

        var data = await Http.GetStreamAsync("Resources/TravelExpensePDFs/" + report.Regulation.UseFile);
        var pdf = PdfReader.Open(data);
        var form = pdf.AcroForm;

        if (pdf.PageCount > 1)
            pdf.Pages.RemoveAt(1);

        var actualAttachmentCount = report.AttachmentCount;

        if (report.VehicleUsed.IsPrivateVehicle()) {
            foreach (var imgRef in report.ImageMapRoute) {
                if (!imgRef.IsSet)
                    continue;

                actualAttachmentCount++;
                await AddAttachment(pdf, imgRef.FileName, StorageManager,
                    $"Anlage #{actualAttachmentCount}: Route für die Fahrt mit einem privaten Fahrzeug");
            }
        }

        if (report.VehicleUsed == Vehicle.PublicTransit) {
            foreach (var imgRef in report.ImagePublicTransitReceipt) {
                if (!imgRef.IsSet)
                    continue;

                actualAttachmentCount++;
                await AddAttachment(pdf, imgRef.FileName, StorageManager,
                    $"Anlage #{actualAttachmentCount}: Beleg für das ÖPV Ticket");
            }
        }

        foreach (var entry in report.OtherCosts) {
            if (entry.ImageReceipt == null)
                continue;

            if (!entry.ImageReceipt.IsSet)
                continue;

            actualAttachmentCount++;
            await AddAttachment(pdf, entry.ImageReceipt.FileName, StorageManager,
                $"Anlage #{actualAttachmentCount}: Beleg für Sonstige Kosten \"{entry.Text}\"");
        }

        await report.Regulation.PdfFormBuilder.BuildPdfFormAsync(form, report, Culture, actualAttachmentCount);

        if (readOnly)
            SetAllFieldsReadOnly(form);

        await using var memoryStream = new MemoryStream();
        pdf.Save(memoryStream);
        await DownloadFileService.DownloadFile(report.GenerateFileName(Culture), memoryStream, "application/octet-stream");
    }

    private static void SetAllFieldsReadOnly(PdfAcroForm form) {
        foreach (var fieldName in form.Fields.Names) {
            form.Fields[fieldName].ReadOnly = true;
        }
    }

    private static void SetField(PdfAcroForm form, string field, string value) {
        form.Fields[field].Value = new PdfString(value, PdfStringEncoding.Unicode);
    }

    private static async Task AddAttachment(PdfDocument pdf, string filename,
        IStorageManagerService storageManager, string reason) {
        var opfsHandle = await storageManager.GetOriginPrivateDirectoryAsync();
        if (!await opfsHandle.FileExists("images"))
            return;

        var imagesDirHandle = await opfsHandle.GetDirectoryHandleAsync("images");
        if (!await imagesDirHandle.FileExists(filename))
            return;

        await using var newStream = await LoadFileAsync(imagesDirHandle, filename);

        var fontSize = 8;
        var font = new XFont("OpenSans", fontSize);

        PdfPage? page = null;
        XGraphics gfx;

        // Finally, File!
        var width = A4Width;
        if (filename.EndsWith(".pdf")) {
            var loadedPdf = PdfReader.Open(newStream, PdfDocumentOpenMode.Import);

            if (loadedPdf.PageCount == 0) {
                page = pdf.AddPage();
            } else {
                foreach (var p in loadedPdf.Pages) {
                    var newPage = pdf.AddPage(p);
                    page ??= newPage;
                }
            }

            gfx = XGraphics.FromPdfPage(page);
        } else if (filename.EndsWith(".png")) {
            page = pdf.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            var image = XImage.FromStream(() => newStream);
            PlaceImage(gfx, page, image, out width);
        } else {
            page = pdf.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString($"Der Dateityp wird nicht unterstützt: {Path.GetExtension(filename)}!",
                font, XBrushes.Black, 10, 30, XStringFormats.TopLeft);
        }

        gfx.DrawRectangle(XBrushes.LightGray, 0, 0, width, fontSize + 6);
        gfx.DrawString(reason, font, XBrushes.Black, 3, 1, XStringFormats.TopLeft);
        gfx.DrawString(filename, font, XBrushes.Black, width - 3, 1, XStringFormats.TopRight);
    }

    private static async Task<MemoryStream> LoadFileAsync(FileSystemDirectoryHandle imagesDirHandle, string filename) {
        var fileStream = await imagesDirHandle.LoadFile(filename);

        // Copy the image into a MemoryStream - this Copy MUST run async because JS doesn't support sync copies
        await using var memStream = new MemoryStream();

        var bytesRead = 0;
        while ((bytesRead = await fileStream.ReadAsync(AttachmentBuffer)) > 0)
            memStream.Write(AttachmentBuffer, 0, bytesRead);

        // But guess what, PDFSharp doesn't want to load an XImage from a MemoryStream if it was copied into it
        var data = memStream.ToArray();
        return new MemoryStream(data);
    }

    private static void PlaceImage(XGraphics gfx, PdfPage page, XImage image, out double width) {
        page.Size = PdfSharpCore.PageSize.A4;
        double scale;

        if (image.PixelWidth > image.PixelHeight && image.PixelWidth > A4Width) {
            page.Orientation = PdfSharpCore.PageOrientation.Landscape;
            scale = Math.Max(1, image.PixelWidth / (A4Height - 80));
            width = A4Height;
        } else {
            scale = Math.Max(1, image.PixelHeight / (A4Height - 20));
            width = A4Width;
        }

        gfx.DrawImage(image, 10, 60, image.PixelWidth / scale, image.PixelHeight / scale);
    }
}