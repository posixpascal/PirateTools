using System;

namespace PirateTools.Models;

public enum Vehicle {
    Undefined,
    PublicTransit,
    Car,
    Motorbike,
    Train,
    Bus,
    Tram,
    LightRail,
    Taxi,
    Other
}

public static class VehicleExtensions {
    public static Vehicle[] All { get; } = Enum.GetValues<Vehicle>();

    public static string GetVehicleName(this Vehicle vehicle) {
        switch (vehicle) {
            case Vehicle.Car: return "PKW";
            case Vehicle.PublicTransit: return "ÖPV";
            case Vehicle.Motorbike: return "Motorrad";
            case Vehicle.Train: return "Zug";
            case Vehicle.Bus: return "Bus";
            case Vehicle.Tram: return "Straßenbahn";
            case Vehicle.LightRail: return "Stadtbahn";
            case Vehicle.Taxi: return "Taxi";
            case Vehicle.Other: return "Sonstiges";
            default: return "-";
        }
    }

    public static bool IsPrivateVehicle(this Vehicle vehicle)
        => vehicle == Vehicle.Car || vehicle == Vehicle.Motorbike;
}