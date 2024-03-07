using PirateTools.Models.Legacy;
using System.Linq;
using System;

namespace PirateTools.Models.Migrations;

public static class TravelExpenseReport_V0_V1_Migration {
    public static TravelExpenseReport_V1 MigrateUp(TravelExpenseReport_V0 oldReport) {
        var report = new TravelExpenseReport_V1();
        report.Id = oldReport.Id;

        if (oldReport.UsedExistingUser && oldReport.Pirate != null)
            report.SetMeta(MetaKeys.PirateId, oldReport.Pirate.Id.ToString());

        report.User = oldReport.Pirate;
        report.Federation = oldReport.Federation;
        report.Function = oldReport.Function;
        report.TravelReason = oldReport.TravelReason;
        report.ResolutionID = oldReport.ResolutionID;
        report.ResolutionDate = oldReport.ResolutionDate;
        report.StartDate = oldReport.StartDate;
        report.EndDate = oldReport.EndDate;
        report.StartTime = oldReport.DepartureTime;
        report.EndTime = oldReport.ArrivalTime;
        report.Destination = oldReport.Destination;
        report.CreationDateTime = new DateTime(oldReport.CreationDateTime, TimeOnly.MinValue);

        if (oldReport.VehicleUsed.IsPrivateVehicle()) {
            var section = report.AddSection<PrivateVehicleTravelSection>();
            section.Date = oldReport.StartDate;
            section.Donate = oldReport.DonateKmFlatRate;
            section.Vehicle = oldReport.VehicleUsed;
            section.Km = oldReport.DrivenKm;

            if (oldReport.ImageMapRoute.Count != 0) {
                section.FileRoute = new FileReference {
                    FileName = oldReport.ImageMapRoute[0].FileName
                };

                foreach (var img in oldReport.ImageMapRoute.Skip(1)) {
                    var s = report.AddSection<PrivateVehicleTravelSection>();
                    s.Date = oldReport.StartDate;
                    s.Vehicle = oldReport.VehicleUsed;
                    s.FileRoute = new FileReference {
                        FileName = img.FileName
                    };
                }
            }
        } else {
            var section = report.AddSection<PublicTransitTravelSection>();
            section.Date = oldReport.StartDate;
            section.Donate = oldReport.DonatePublicTransitCosts;
            section.Vehicle = oldReport.VehicleUsed;
            section.Cost = oldReport.PublicTransitCosts;

            if (oldReport.ImagePublicTransitReceipt.Count != 0) {
                section.FileReceipt = new FileReference {
                    FileName = oldReport.ImagePublicTransitReceipt[0].FileName
                };

                foreach (var img in oldReport.ImagePublicTransitReceipt.Skip(1)) {
                    var s = report.AddSection<PublicTransitTravelSection>();
                    s.Date = oldReport.StartDate;
                    s.Vehicle = oldReport.VehicleUsed;
                    s.FileReceipt = new FileReference {
                        FileName = img.FileName
                    };
                }
            }
        }

        return report;
    }
}