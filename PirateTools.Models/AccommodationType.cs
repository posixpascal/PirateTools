namespace PirateTools.Models;

public enum AccommodationType {
    None,
    Specified,
    FlatRate
}

public static class AccommodationTypeExtensions {
    public static string GetAccommodationTypeName(this AccommodationType type) {
        switch (type) {
            case AccommodationType.Specified: return "Nach Beleg";
            case AccommodationType.FlatRate: return "Pauschal";
            default: return "-";
        }
    }
}