using PdfSharpCore.Pdf.AcroForms;
using PirateTools.Models.Legacy;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace PirateTools.Models.PDF;

public class DefaultPdfFormBuilder : PdfFormBuilder {
    public override Task BuildPdfFormAsync(PdfAcroForm form, TravelExpenseReport_V0 report,
        CultureInfo culture, int attachmentCount) {
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

            if (!report.IsDonatingAll()) {
                SetField(form, "PiratIBAN", report.Pirate.IBAN ?? "");
                SetField(form, "PiratBIC", report.Pirate.BIC ?? "");
            }
        }

        SetField(form, "Funktion", report.Function);
        SetField(form, "Zweck", report.TravelReason);

        if (!string.IsNullOrEmpty(report.ResolutionID)) {
            SetField(form, "BeschlussNr", report.ResolutionID);
            SetField(form, "BeschlussDatum", report.ResolutionDate?.ToString(culture) ?? "");
        }

        SetField(form, "ReiseStartDatum", report.StartDate.ToString(culture));
        SetField(form, "ReiseEndeDatum", report.EndDate.ToString(culture));

        SetField(form, "ReiseStartUhrzeit", report.DepartureTime.ToString(culture));
        SetField(form, "ReiseEndeUhrzeit", report.ArrivalTime.ToString(culture));

        SetField(form, "ReiseZiel", report.Destination);

        if (report.VehicleUsed.IsPrivateVehicle()) {
            SetField(form, "KmGefahren", report.DrivenKm.ToString("N2", culture));
            SetField(form, "KmPauschale", report.DrivenCompensation.ToString("N2", culture));
        } else if (report.VehicleUsed == Vehicle.PublicTransit) {
            SetField(form, "TicketKosten", report.PublicTransitCosts.ToString("N2", culture));
        }

        SetField(form, "AnAbreiseTage", report.NumberShortDays.ToString());
        SetField(form, "VolleTage", report.NumberFullDays.ToString());
        SetField(form, "GeldAnAbreiseTage", report.ShortDaysCompensation.ToString("N2", culture));
        SetField(form, "GeldVolleTage", report.FullDaysCompensation.ToString("N2", culture));

        SetField(form, "VerpflegungSumme", (report.ShortDaysCompensation + report.FullDaysCompensation).ToString("N2", culture));

        if (report.AccommodationType == AccommodationType.FlatRate) {
            SetField(form, "HotelPauschal", report.CalculatedAccommodationCosts.ToString("N2", culture));
        } else if (report.AccommodationType == AccommodationType.Specified) {
            SetField(form, "HotelNachBeleg", report.CalculatedAccommodationCosts.ToString("N2", culture));
        }

        // TODO: Add system to add more than 3 via attachment page
        for (int i = 0; i < Math.Min(report.OtherCosts.Count, 3); i++) {
            var addition = report.OtherCosts[i];
            SetField(form, $"Sonstiges{i + 1}", addition.Text);
            SetField(form, $"SonstigesKosten{i + 1}", addition.Cost.ToString("N2", culture));
        }

        SetField(form, "SummeGesamt", report.TotalCosts.ToString("N2", culture));
        SetField(form, "Spendenanteil", report.CalculateDonateAmount().ToString("N2", culture));
        SetField(form, "Auszahlbetrag", (report.TotalCosts - report.CalculateDonateAmount()).ToString("N2", culture));

        SetField(form, "UnterschriftDatum", DateOnly.FromDateTime(DateTime.Now).ToString(culture));

        SetField(form, "AnzahlAnlagen", attachmentCount.ToString());

        return Task.CompletedTask;
    }
}