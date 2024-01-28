using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PirateTools.Models;

public class Federation {
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ShortType { get; set; }
    public string LongType { get; set; }

    public string ShortName { get; set; }
    public string Name { get; set; }

    public Federation? Parent { get; set; }

    public List<ChairMember> Chairs { get; set; } = [];

    public List<TravelExpenseRegulation> TravelExpenseRegulations { get; set; } = [];

    public string? TreasurerAddress { get; set; }

    [JsonIgnore]
    public bool CanBeUsedForReport => !string.IsNullOrEmpty(TreasurerAddress) && TravelExpenseRegulations.Count > 0;

    [JsonConstructor]
    public Federation() { }

    public Federation(string shortType, string longType, string shortName, string name) {
        ShortType = shortType;
        LongType = longType;
        ShortName = shortName;
        Name = name;
    }

    public string TravelExpenseClaimHead() => $"{TreasurerAddress}";

    public string FullName() => $"{LongType} {Name}";
    public string Identifier() => ShortType + ShortName;
}