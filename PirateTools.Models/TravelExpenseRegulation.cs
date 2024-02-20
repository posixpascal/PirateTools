using PirateTools.Models.PDF;
using System;
using System.Collections.Generic;

namespace PirateTools.Models;

public class TravelExpenseRegulation {
    //Base Data
    public required string UseFile { get; set; } = "PPDE_2022.pdf";
    public required DateOnly AvailableFrom { get; set; }

    public PdfFormBuilder PdfFormBuilder { get; set; } = new DefaultPdfFormBuilder();

    //Actual regulations
    public double MotorBikeCompensation { get; set; } = 0.13;

    public double ShortDaysCompensation { get; set; } = 14;
    public double FullDaysCompensation { get; set; } = 28;

    public TimeSpan ExpectedSubmissionAfterTravel { get; set; } = TimeSpan.FromDays(30);
    public DateOnly LatestSubmissionDate { get; set; } = new DateOnly(1, 2, 15);

    public bool HasFieldForResolutionID { get; set; } = true;

    public bool RequiresSpecificOtherCosts { get; set; }
    public List<string> OtherCostTemplates { get; set; } = [];

    public bool UseGranularPublicTransitCosts { get; set; }

    public static readonly TravelExpenseRegulation Default2022 = new() {
        UseFile = "PPDE_2022.pdf",
        AvailableFrom = new DateOnly(2022, 4, 1)
    };

    public static readonly TravelExpenseRegulation Default2024 = new() {
        UseFile = "PPDE_2024.pdf",
        AvailableFrom = new DateOnly(2024, 1, 1),
        MotorBikeCompensation = 0.2
    };
}