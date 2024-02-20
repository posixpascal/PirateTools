using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace PirateTools.Models;

public class TravelExpenseReport {
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool UsedExistingUser { get; set; }

    public Pirate? Pirate { get; set; }
    public Federation? Federation { get; set; }

    [JsonIgnore]
    public TravelExpenseRegulation Regulation { get; set; } = TravelExpenseRegulation.Default2022;

    public string Function { get; set; } = "";
    public string TravelReason { get; set; } = "";

    public string ResolutionID { get; set; } = "";
    public DateOnly? ResolutionDate { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }

    [JsonIgnore]
    public DateTime StartDateTime => new(StartDate, DepartureTime);
    [JsonIgnore]
    public DateTime EndDateTime => new(EndDate, ArrivalTime);

    [JsonIgnore]
    public int NightsStayed => EndDate.DayNumber - StartDate.DayNumber;

    [JsonIgnore]
    public int NumberShortDays {
        get {
            //single day travel => only counts if >= 8 hrs
            if (NightsStayed == 0)
                return (ArrivalTime - DepartureTime).TotalHours < 8 ? 0 : 1;

            return 2;
        }
    }

    [JsonIgnore]
    public int NumberFullDays => Math.Max(0, NightsStayed - 1);

    [JsonIgnore]
    public double ShortDaysCompensation => NumberShortDays * Regulation.ShortDaysCompensation;

    [JsonIgnore]
    public double FullDaysCompensation => NumberFullDays * Regulation.FullDaysCompensation;

    public string Destination { get; set; } = "";

    public Vehicle VehicleUsed { get; set; }
    public double DrivenKm { get; set; }

    public double PublicTransitCosts { get; set; }

    // More specific cost, used for some older Regulations
    public double PublicTransitTrain { get; set; }
    public double PublicTransitBusTramLRT { get; set; }
    public double PublicTransitTaxi { get; set; }
    public double PublicTransitOther { get; set; }

    public List<ImageReference> ImagePublicTransitReceipt { get; set; } = [];
    public List<ImageReference> ImageMapRoute { get; set; } = [];

    [JsonIgnore]
    public double DrivenCompensation => CalculateDrivenCompensation();

    public AccommodationType AccommodationType { get; set; }

    public double AccommodationCosts { get; set; }

    public List<ImageReference> ImageAccommodationReceipt { get; set; } = [];

    [JsonIgnore]
    public double NightsStayedCompensation
        => AccommodationType == AccommodationType.FlatRate ? (NightsStayed * 20d) : 0;

    [JsonIgnore]
    public double CalculatedAccommodationCosts {
        get {
            if (AccommodationType == AccommodationType.FlatRate)
                return NightsStayedCompensation;

            return AccommodationCosts;
        }
    }

    public List<AdditionalCosts> OtherCosts { get; set; } = [];

    public bool DonateAll { get; set; }
    public bool DonatePublicTransitCosts { get; set; }
    public bool DonateKmFlatRate { get; set; }
    public bool DonateFoodFlatRateShortDays { get; set; }
    public bool DonateFoodFlatRateFullDays { get; set; }
    public bool DonateAccommodationCosts { get; set; }

    public bool DonateCustomAmount { get; set; }
    public double DonateAmountCustom { get; set; }

    public int AttachmentCount { get; set; }
    public DateOnly CreationDateTime { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [JsonIgnore]
    public double TotalCosts
        => PublicTransitCosts + DrivenCompensation + ShortDaysCompensation
        + FullDaysCompensation + CalculatedAccommodationCosts + OtherCosts.Sum(c => c.Cost);

    public bool IsDonatingAllOtherCosts() => OtherCosts.All(c => c.Donate);

    public bool IsDonatingAll() {
        if (DonateAll)
            return true;
        if (DonateCustomAmount)
            return DonateAmountCustom == TotalCosts;

        var donateTransit = DonatePublicTransitCosts || DonateKmFlatRate;
        return donateTransit
            && DonateFoodFlatRateFullDays
            && DonateFoodFlatRateShortDays
            && DonateAccommodationCosts
            && IsDonatingAllOtherCosts();
    }

    public double CalculateDonateAmount() {
        if (DonateAll)
            return TotalCosts;
        if (DonateAmountCustom != 0)
            return Math.Min(DonateAmountCustom, TotalCosts);

        var amount = 0d;
        if (DonatePublicTransitCosts)
            amount += PublicTransitCosts;
        if (DonateKmFlatRate)
            amount += DrivenCompensation;
        if (DonateFoodFlatRateShortDays)
            amount += ShortDaysCompensation;
        if (DonateFoodFlatRateFullDays)
            amount += FullDaysCompensation;
        if (DonateAccommodationCosts)
            amount += CalculatedAccommodationCosts;

        foreach (var additional in OtherCosts) {
            if (additional.Donate)
                amount += additional.Cost;
        }

        return amount;
    }

    public double CalculateDrivenCompensation() {
        if (!VehicleUsed.IsPrivateVehicle())
            return 0;

        return (VehicleUsed == Vehicle.Car ? 0.3 : Regulation.MotorBikeCompensation) * DrivenKm;
    }

    public double GetVehicleOrPublicTransitCosts() {
        if (VehicleUsed == Vehicle.PublicTransit)
            return GetPublicTransitCosts();

        if (VehicleUsed == Vehicle.Undefined)
            return 0;

        return CalculateDrivenCompensation();
    }

    public double GetPublicTransitCosts() {
        if (VehicleUsed != Vehicle.PublicTransit)
            return 0;

        return PublicTransitTrain + PublicTransitBusTramLRT + PublicTransitTaxi + PublicTransitOther;
    }

    public string GenerateFileName(CultureInfo culture) {
        string fileName;
        if (Pirate == null || string.IsNullOrEmpty(Destination)) {
            fileName = Id.ToString();
        } else {
            fileName = Pirate.Name + " - " + Destination + " - " + StartDate.ToString(culture);
        }

        return fileName + ".pdf";
    }

    public void FigureOutRegulation() {
        if (Federation == null || Federation.TravelExpenseRegulations.Count == 0) {
            Regulation = TravelExpenseRegulation.Default2022;
            Console.WriteLine($"Federation: {Federation?.Name ?? "null"}, " +
                $"RegulationCount: {Federation?.TravelExpenseRegulations.Count ?? 0}");
            return;
        }

        //Find the newest regulation that fits our StartDate
        foreach (var regulation in Federation.TravelExpenseRegulations
            .OrderByDescending(r => r.AvailableFrom.DayNumber)) {
            if (regulation.AvailableFrom.DayNumber > StartDate.DayNumber)
                continue;

            Regulation = regulation;
            Console.WriteLine("Found a Regulation: " + Regulation.UseFile);
            break;
        }
    }

    /// <summary>
    /// Clones this TravelExpenseReport but will assign a new Guid!
    /// </summary>
    public TravelExpenseReport Clone() {
        return new TravelExpenseReport() {
            Id = Guid.NewGuid(),
            StartDate = StartDate,
            EndDate = EndDate,
            Destination = Destination,
            VehicleUsed = VehicleUsed,
            DrivenKm = DrivenKm,
            PublicTransitCosts = PublicTransitCosts,
            OtherCosts = OtherCosts.ConvertAll(c => c.Clone()),
            DonateAll = DonateAll,
            DonateAmountCustom = DonateAmountCustom,
            DonatePublicTransitCosts = DonatePublicTransitCosts,
            DonateKmFlatRate = DonateKmFlatRate,
            DonateFoodFlatRateFullDays = DonateFoodFlatRateFullDays,
            DonateFoodFlatRateShortDays = DonateFoodFlatRateShortDays,
            DonateAccommodationCosts = DonateAccommodationCosts,
            Federation = Federation,
            Pirate = Pirate,
            AttachmentCount = AttachmentCount,
            TravelReason = TravelReason,
        };
    }

    public class AdditionalCosts {
        public string Text { get; set; } = "";
        public double Cost { get; set; }
        public bool Donate { get; set; }
        public ImageReference ImageReceipt { get; set; } = new();

        public AdditionalCosts Clone() {
            return new AdditionalCosts() {
                Text = Text,
                Cost = Cost,
                Donate = Donate,
                ImageReceipt = ImageReceipt.Clone()
            };
        }
    }

    public class ImageReference {
        [MemberNotNullWhen(true, nameof(FileName))]
        public bool IsSet => FileName != null;

        public string? FileName { get; set; }

        public ImageReference Clone() {
            return new ImageReference() {
                FileName = FileName
            };
        }
    }
}