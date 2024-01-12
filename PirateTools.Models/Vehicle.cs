using System;

namespace PirateTools.Models;

public enum Vehicle {
    Undefined,
    PublicTransit,
    Car,
    Motorbike
}

public static class VehicleExtensions {
    public static Vehicle[] All { get; } = Enum.GetValues<Vehicle>();

    public static string GetVehicleName(this Vehicle vehicle) {
        switch (vehicle) {
            case Vehicle.Car: return "PKW";
            case Vehicle.PublicTransit: return "ÖPV";
            case Vehicle.Motorbike: return "Motorrad";
            default: return "-";
        }
    }

    public static bool IsPrivateVehicle(this Vehicle vehicle)
        => vehicle == Vehicle.Car || vehicle == Vehicle.Motorbike;
}