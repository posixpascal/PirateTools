using System;
using System.Text.Json.Serialization;

namespace PirateTools.Models;

[JsonDerivedType(typeof(PrivateVehicleTravelSection), 0)]
[JsonDerivedType(typeof(PublicTransitTravelSection), 1)]
[JsonDerivedType(typeof(FlatRateHotelSection), 2)]
[JsonDerivedType(typeof(HotelSection), 3)]
[JsonDerivedType(typeof(PaidMealSection), 4)]
[JsonDerivedType(typeof(AdditionalCostSection), 5)]
public abstract class TravelSectionBase {
    public string Note { get; set; } = "";
    public DateOnly Date { get; set; }

    public bool Donate { get; set; }
    public abstract bool CanDonate { get; }
}

public class PrivateVehicleTravelSection : TravelSectionBase {
    public double Km { get; set; }
    public Vehicle Vehicle { get; set; }
    public FileReference FileRoute { get; set; } = new();

    public override bool CanDonate => true;
}

public class PublicTransitTravelSection : TravelSectionBase {
    public double Cost { get; set; }
    public Vehicle Vehicle { get; set; }
    public FileReference FileReceipt { get; set; } = new();

    public override bool CanDonate => true;
}

public class FlatRateHotelSection : TravelSectionBase {
    public DateOnly EndDate { get; set; }

    public override bool CanDonate => true;
}

public class HotelSection : TravelSectionBase {
    public DateOnly EndDate { get; set; }
    public double Cost { get; set; }
    public bool IncludesBreakfast { get; set; }
    public FileReference FileReceipt { get; set; } = new();

    public override bool CanDonate => true;
}

public class PaidMealSection : TravelSectionBase {
    public MealType Type { get; set; }

    public override bool CanDonate => false;

    public enum MealType {
        Breakfast,
        Lunch,
        Dinner
    }
}

public class AdditionalCostSection : TravelSectionBase {
    public double Cost { get; set; }

    public override bool CanDonate => true;
}