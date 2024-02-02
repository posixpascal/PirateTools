using PdfSharpCore.Pdf.AcroForms;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace PirateTools.Models.PDF;

public class LVHE_1_PdfFormBuilder : PdfFormBuilder {
    public override Task BuildPdfFormAsync(PdfAcroForm form, TravelExpenseReport report,
        CultureInfo culture, int attachmentCount) {
        if (report.Pirate != null) {
            SetField(form, "Name", report.Pirate.Name);
            SetField(form, "Adresse1", $"{report.Pirate.Address.LocalIdentifier} {report.Pirate.Address.HouseIdentifier}");
            SetField(form, "Adresse2", $"{report.Pirate.Address.PostCode} {report.Pirate.Address.City}");

            if (!report.IsDonatingAll()) {
                SetField(form, "PiratIBAN", report.Pirate.IBAN ?? "");
                SetField(form, "PiratBIC", report.Pirate.BIC ?? "");
            }
        }

        SetField(form, "Funktion", report.Function);
        SetField(form, "Zweck", report.TravelReason);

        if (report.NightsStayed == 0) {
            SetField(form, "Von", $"{report.StartDate.ToString(culture)} ({report.DepartureTime.ToString(culture)})");
            SetField(form, "Bis", $"{report.EndDate.ToString(culture)} ({report.ArrivalTime.ToString(culture)})");
        } else {
            SetField(form, "Von", report.StartDate.ToString(culture));
            SetField(form, "Bis", report.EndDate.ToString(culture));
        }

        SetField(form, "Ziel", report.Destination);

        if (report.VehicleUsed.IsPrivateVehicle()) {
            SetField(form, "StreckeKm", report.DrivenKm.ToString("N2", culture));
            SetField(form, "StreckeKosten", report.DrivenCompensation.ToString("C2", culture));
        } else if (report.VehicleUsed == Vehicle.PublicTransit) {
            SetField(form, "Bahnkosten", report.PublicTransitTrain.ToString("C2", culture));
            SetField(form, "BusTramLRT", report.PublicTransitBusTramLRT.ToString("C2", culture));
            SetField(form, "Taxi", report.PublicTransitTaxi.ToString("C2", culture));
            SetField(form, "AnderesTransportmittel", report.PublicTransitOther.ToString("C2", culture));
        }

        if (report.VehicleUsed.IsPrivateVehicle()) {
            if (report.ImageMapRoute.Count > 0) {
                SetField(form, "RoutenplanJa", "X");
            } else {
                SetField(form, "RoutenplanNein", "X");
            }
        }

        if (report.NumberShortDays > 0) {
            if (report.NightsStayed == 0) {
                SetField(form, "VerpflegungEinTagAnzahl", report.NumberShortDays.ToString());
                SetField(form, "VerpflegungEinTagKosten", report.ShortDaysCompensation.ToString("C2", culture));
            } else if (report.NightsStayed == 1) {
                SetField(form, "VerpflegungTeilAnzahl", report.NumberShortDays.ToString());
                SetField(form, "VerpflegungTeilKosten", report.ShortDaysCompensation.ToString("C2", culture));
            } else {
                SetField(form, "VerpflegungTeilAnzahl", report.NumberShortDays.ToString());
                SetField(form, "VerpflegungVollAnzahl", report.NumberFullDays.ToString());

                SetField(form, "VerpflegungTeilKosten", report.ShortDaysCompensation.ToString("C2", culture));
                SetField(form, "VerpflegungVollKosten", report.FullDaysCompensation.ToString("C2", culture));
            }
        }

        SetField(form, "VerpflegungSumme", (report.ShortDaysCompensation + report.FullDaysCompensation).ToString("C2", culture));

        if (report.AccommodationType == AccommodationType.FlatRate) {
            SetField(form, "NachtPauschal", report.CalculatedAccommodationCosts.ToString("C2", culture));
        } else if (report.AccommodationType == AccommodationType.Specified) {
            SetField(form, "NachBeleg", report.CalculatedAccommodationCosts.ToString("C2", culture));
        }

        SetField(form, "SummeFahrtkosten", report.GetVehicleOrPublicTransitCosts().ToString("C2", culture));
        SetField(form, "SummeVerpflegung", (report.ShortDaysCompensation + report.FullDaysCompensation).ToString("C2", culture));
        SetField(form, "SummeNacht", report.CalculatedAccommodationCosts.ToString("C2", culture));

        if (report.OtherCosts.Count >= 1 && report.OtherCosts[0].Cost != 0)
            SetField(form, "Nebenkosten", report.OtherCosts[0].Cost.ToString("C2", culture));
        if (report.OtherCosts.Count >= 2 && report.OtherCosts[1].Cost != 0)
            SetField(form, "ParkenTelefonSonstiges", report.OtherCosts[1].Cost.ToString("C2", culture));

        SetField(form, "SummeGesamt", report.TotalCosts.ToString("C2", culture));
        SetField(form, "Spendenanteil", report.CalculateDonateAmount().ToString("C2", culture));
        SetField(form, "Auszahlanteil", (report.TotalCosts - report.CalculateDonateAmount()).ToString("C2", culture));

        SetField(form, "DatumUnterschrift", DateOnly.FromDateTime(DateTime.Now).ToString(culture));

        return Task.CompletedTask;
    }
}