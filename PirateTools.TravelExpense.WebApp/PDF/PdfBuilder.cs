using BlazorDownloadFile;
using KristofferStrube.Blazor.FileSystem;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Utils;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.TravelExpense.WebApp.Utility;
using SixLabors.ImageSharp;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.PDF;

public static class PdfBuilder {
    private static readonly byte[] AttachmentBuffer = new byte[5_000_000];

    public static async Task BuildPdfAsync(TravelExpenseReport report, FontService FontService,
        HttpClient Http, CultureInfo Culture, IBlazorDownloadFileService DownloadFileService,
        IStorageManagerService StorageManager) {
        await FontService.LoadFontAsync("OpenSans-Regular.ttf");
        await FontService.LoadFontAsync("OpenSans-Bold.ttf");
        await FontService.LoadFontAsync("OpenSans-Italic.ttf");
        await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");

        var data = await Http.GetStreamAsync("Resources/TravelExpensePDFs/LVNDS.pdf");
        var pdf = PdfReader.Open(data);
        var form = pdf.AcroForm;

        if (report.Federation != null) {
            SetField(form, "SchatzmeisterAdresse", report.Federation.TravelExpenseClaimHead());
        }

        if (report.Pirate != null) {
            SetField(form, "Name", report.Pirate.Name);
            SetField(form, "Adresse1", $"{report.Pirate.Address.LocalIdentifier} {report.Pirate.Address.HouseIdentifier}");
            SetField(form, "Adresse2", $"{report.Pirate.Address.PostCode} {report.Pirate.Address.City}");
            SetField(form, "MitgliedsNr", report.Pirate.MemberID.ToString());
            SetField(form, "Landesverband", report.Pirate.Federation.Name);

            SetField(form, "EMail", report.Pirate.EMail ?? "");
            SetField(form, "PiratIBAN", report.Pirate.IBAN ?? "");
            SetField(form, "PiratBIC", report.Pirate.BIC ?? "");
        }

        SetField(form, "Funktion", report.Function);
        SetField(form, "Zweck", report.TravelReason);

        SetField(form, "BeschlussNr", report.ResolutionID);
        SetField(form, "BeschlussDatum", report.ResolutionDate.ToString(Culture));

        SetField(form, "ReiseStartDatum", report.StartDate.ToString(Culture));
        SetField(form, "ReiseEndeDatum", report.EndDate.ToString(Culture));

        SetField(form, "ReiseStartUhrzeit", report.DepartureTime.ToString(Culture));
        SetField(form, "ReiseEndeUhrzeit", report.ArrivalTime.ToString(Culture));

        SetField(form, "ReiseZiel", report.Destination);

        if (report.VehicleUsed.IsPrivateVehicle()) {
            SetField(form, "KmGefahren", report.DrivenKm.ToString("N2", Culture));
            SetField(form, "KmPauschale", report.DrivenCompensation.ToString("N2", Culture));
        } else if (report.VehicleUsed == Vehicle.PublicTransit) {
            SetField(form, "TicketKosten", report.PublicTransitCosts.ToString("N2", Culture));
        }

        SetField(form, "AnAbreiseTage", report.NumberShortDays.ToString());
        SetField(form, "VolleTage", report.NumberFullDays.ToString());
        SetField(form, "GeldAnAbreiseTage", report.ShortDaysCompensation.ToString("N2", Culture));
        SetField(form, "GeldVolleTage", report.FullDaysCompensation.ToString("N2", Culture));

        SetField(form, "VerpflegungSumme", (report.ShortDaysCompensation + report.FullDaysCompensation).ToString("N2", Culture));

        if (report.AccommodationType == AccommodationType.FlatRate) {
            SetField(form, "HotelPauschal", report.CalculatedAccommodationCosts.ToString("N2", Culture));
        } else if (report.AccommodationType == AccommodationType.Specified) {
            SetField(form, "HotelNachBeleg", report.CalculatedAccommodationCosts.ToString("N2", Culture));
        }

        // TODO: Add system to add more than 3 via attachment page
        for (int i = 0; i < Math.Min(report.OtherCosts.Count, 3); i++) {
            var addition = report.OtherCosts[i];
            SetField(form, $"Sonstiges{i + 1}", addition.Text);
            SetField(form, $"SonstigesKosten{i + 1}", addition.Cost.ToString("N2", Culture));
        }

        SetField(form, "SummeGesamt", report.TotalCosts.ToString("N2", Culture));
        SetField(form, "Spendenanteil", report.CalculateDonateAmount().ToString("N2", Culture));
        SetField(form, "Auszahlbetrag", (report.TotalCosts - report.CalculateDonateAmount()).ToString("N2", Culture));

        SetField(form, "UnterschriftDatum", report.CreationDateTime.ToString(Culture));

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

        SetField(form, "AnzahlAnlagen", actualAttachmentCount.ToString());

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

        var attachmentPage = pdf.AddPage();
        var fileStream = await imagesDirHandle.LoadFile(filename);

        // Copy the image into a MemoryStream - this Copy MUST run async because JS doesn't support sync copies
        await using var memStream = new MemoryStream();

        var bytesRead = 0;
        while ((bytesRead = await fileStream.ReadAsync(AttachmentBuffer)) > 0)
            memStream.Write(AttachmentBuffer, 0, bytesRead);

        // But guess what, PDFSharp doesn't want to load an XImage from a MemoryStream if it was copied into it
        var data = memStream.ToArray();
        await using var newStream = new MemoryStream(data);

        // Finally, Image!
        var gfx = XGraphics.FromPdfPage(attachmentPage);
        var image = XImage.FromStream(() => newStream);
        PlaceImage(gfx, attachmentPage, image);

        // And some text ... that's the easy part
        var font = new XFont("OpenSans", 16);
        gfx.DrawString(reason, font, XBrushes.Black, 10, 10, XStringFormats.TopLeft);
        gfx.DrawString(filename, font, XBrushes.Black, 10, 30, XStringFormats.TopLeft);
    }

    private static void PlaceImage(XGraphics gfx, PdfPage page, XImage image) {
        const double A4Height = 842 - 80;
        const double A4Width = 595 - 20;

        page.Size = PdfSharpCore.PageSize.A4;
        double scale;

        if (image.PixelWidth > image.PixelHeight && image.PixelWidth > A4Width) {
            page.Orientation = PdfSharpCore.PageOrientation.Landscape;
            scale = Math.Max(1, image.PixelWidth / A4Height);
        } else {
            scale = Math.Max(1, image.PixelHeight / A4Height);
        }

        gfx.DrawImage(image, 10, 60, image.PixelWidth / scale, image.PixelHeight / scale);
    }
}