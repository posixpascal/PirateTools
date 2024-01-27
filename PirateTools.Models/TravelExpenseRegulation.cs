using System;

namespace PirateTools.Models;

public class TravelExpenseRegulation {
    //Base Data
    public string UseFile { get; set; } = "PPDE.pdf";
    public DateOnly AvailableFrom { get; set; }

    //Actual regulations
    public double MotorBikeCompensation { get; set; }

    public static readonly TravelExpenseRegulation Default = new() {
        UseFile = "PPDE.pdf",
        AvailableFrom = new DateOnly(2022, 4, 1),
        MotorBikeCompensation = 0.13
    };
}