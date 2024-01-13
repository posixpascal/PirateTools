using BlazorDownloadFile;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.PDF;

public static class PdfBuilder {
    public static async Task BuildPdfAsync(TravelExpenseReport report, FontService FontService,
        HttpClient Http, CultureInfo Culture, IBlazorDownloadFileService DownloadFileService) {
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

        SetField(form, "AnzahlAnlagen", report.AttachmentCount.ToString());
        SetField(form, "UnterschriftDatum", report.CreationDateTime.ToString(Culture));

        await using var memoryStream = new MemoryStream();
        pdf.Save(memoryStream);
        await DownloadFileService.DownloadFile(report.GenerateFileName(Culture), memoryStream, "application/octet-stream");
    }

    private static void SetField(PdfAcroForm form, string field, string value) {
        form.Fields[field].Value = new PdfString(value, PdfStringEncoding.Unicode);
    }
}