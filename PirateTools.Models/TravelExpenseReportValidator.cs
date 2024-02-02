using System.Collections.Generic;
using System.Linq;

namespace PirateTools.Models;

public static class TravelExpenseReportValidator {
    public static IEnumerable<ReportIssue> ValidateReport(TravelExpenseReport report) {
        foreach (var ri in ValidateReportErrors(report))
            yield return ri;
        foreach (var ri in ValidateReportWarnings(report))
            yield return ri;
        foreach (var ri in ValidateReportInfos(report))
            yield return ri;
    }

    public static IEnumerable<ReportIssue> ValidateReportErrors(TravelExpenseReport report) {
        if (report.Pirate == null) {
            yield return ReportIssue.Error("Es wurde keine Person angegeben die gereist ist.", "StepUserSelection");
        } else {
            if (string.IsNullOrEmpty(report.Pirate.Name))
                yield return ReportIssue.Error("Es wurde keine Person angegeben die gereist ist.", "StepUserDataEntry");
            if (!report.Pirate.Address.IsValidAddress())
                yield return ReportIssue.Error("Die Adresse ist nicht vollständig.", "StepUserDataEntry");
            if (report.Pirate.MemberID == 0)
                yield return ReportIssue.Error("Die Mitgliedsnummer wurde nicht angegeben.", "StepUserDataEntry");

            if (!report.IsDonatingAll() && string.IsNullOrEmpty(report.Pirate.IBAN))
                yield return ReportIssue.Error("Die IBAN wurde nicht angegeben.", "StepUserDataEntry");
        }

        if (string.IsNullOrEmpty(report.TravelReason))
            yield return ReportIssue.Error("Es wurde kein Zweck angegeben.", "StepExpenseBaseDataEntry");

        var vehicleCostStep = $"StepTravelCostsVehicle/{(int)report.VehicleUsed}";
        if (report.VehicleUsed.IsPrivateVehicle() && report.DrivenKm > 0 && !report.ImageMapRoute.Any(ir => ir.IsSet))
            yield return ReportIssue.Error("Es fehlt ein Bild der Strecke für die Nutzung eines privaten Fahrzeuges.", vehicleCostStep);
        if (report.VehicleUsed == Vehicle.PublicTransit && !report.ImagePublicTransitReceipt.Any(ir => ir.IsSet))
            yield return ReportIssue.Error("Es fehlt ein Beleg für die Nutzung des ÖPV!", "StepTravelCostsPublicTransit");
        if (report.OtherCosts.Any(oc => oc.Cost > 0 && !oc.ImageReceipt.IsSet))
            yield return ReportIssue.Error("Es fehlt mindestens ein Beleg für sonstige Kosten!", "StepOtherCostsOverview");

        if (report.TotalCosts == 0)
            yield return ReportIssue.Error("Dieser Reisekostenantrag hat keine Kosten!");

        if ((report.EndDateTime - report.StartDateTime).TotalSeconds < 0)
            yield return ReportIssue.Error("Das Ende der Reise liegt vor dem Anfang der Reise!", "StepExpenseBaseDataEntry");
    }

    public static IEnumerable<ReportIssue> ValidateReportWarnings(TravelExpenseReport report) {
        var vehicleCostStep = $"StepTravelCostsVehicle/{(int)report.VehicleUsed}";
        if (report.VehicleUsed.IsPrivateVehicle() && report.DrivenKm == 0)
            yield return ReportIssue.Warning("Es wurde die Nutzung eines privaten Fahrzeugs angegeben, die gefahrene Distanz ist aber 0.", vehicleCostStep);
        if (report.VehicleUsed == Vehicle.PublicTransit && report.GetPublicTransitCosts() == 0)
            yield return ReportIssue.Warning("Es wurde die Nutzung des ÖPV angegeben, die Kosten sind aber 0.", "StepTravelCostsPublicTransit");

        if ((report.EndDateTime - report.StartDateTime).TotalSeconds == 0)
            yield return ReportIssue.Warning("Die Reise ist 0 Sekunden lang.", "StepExpenseBaseDataEntry");
    }

    public static IEnumerable<ReportIssue> ValidateReportInfos(TravelExpenseReport report) {
        if (report.NightsStayed > 0 && report.AccommodationType == AccommodationType.None)
            yield return ReportIssue.Info("Die Reise ging über eine oder mehr Nächte, es wurde aber angegeben das es keine Übernachtungskosten gibt.", "StepNightlyCostTypeSelection");

        if (report.AttachmentCount > 0)
            yield return ReportIssue.Info("Die Anzahl der zusätzlichen Anhänge ist > 0, bitte denke daran diese noch anzuhängen oder bei der Abgabe beizufügen!");
    }
}