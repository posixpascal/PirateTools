using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PirateTools.Models;

public static class TravelExpenseReportUtil {
    private static readonly HashSet<char> IllegalPathChars = [.. Path.GetInvalidFileNameChars()];

    public static DateTime GetStartDateTime(this TravelExpenseReport_V1 report)
        => new(report.StartDate, report.StartTime);

    public static DateTime GetEndDateTime(this TravelExpenseReport_V1 report)
        => new(report.EndDate, report.EndTime);

    public static int GetTripNights(this TravelExpenseReport_V1 report)
        => report.EndDate.DayNumber - report.StartDate.DayNumber;

    public static int GetTripPartialDays(this TravelExpenseReport_V1 report) {
        // If the trip was less than 8 hrs we may only count a partial day if the trip had a hotel section
        if ((report.GetEndDateTime() - report.GetStartDateTime()).TotalHours <= 8)
            return report.TravelSections.Any(s => s is HotelSection || s is FlatRateHotelSection) ? 1 : 0;

        return 2;
    }

    public static int GetTripFullDays(this TravelExpenseReport_V1 report)
        => Math.Max(0, report.GetTripNights() - 1);

    public static string GenerateFileName(this TravelExpenseReport_V1 report, CultureInfo culture) {
        string fileName;
        if (report.User == null || string.IsNullOrEmpty(report.Destination)) {
            fileName = report.Id.ToString();
        } else {
            fileName = report.User.Name + " - " + report.Destination + " - " + report.StartDate.ToString(culture);
        }

        fileName = string.Concat(fileName.Where(c => !IllegalPathChars.Contains(c)));
        return fileName + ".pdf";
    }
}