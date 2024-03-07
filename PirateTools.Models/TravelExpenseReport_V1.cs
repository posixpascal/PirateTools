using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace PirateTools.Models;

public class TravelExpenseReport_V1 {
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonInclude]
    private Dictionary<string, string> _metadata = [];
    [JsonIgnore]
    public IReadOnlyDictionary<string, string> Metadata => _metadata;

    public Pirate? User { get; set; }
    public Federation? Federation { get; set; }

    public string Function { get; set; } = "";
    public string TravelReason { get; set; } = "";
    public string Destination { get; set; } = "";

    public double DonateAmountCustom { get; set; }

    public string ResolutionID { get; set; } = "";
    public DateOnly? ResolutionDate { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [JsonInclude]
    private List<TravelSectionBase> _travelSections = [];
    [JsonIgnore]
    public IReadOnlyList<TravelSectionBase> TravelSections => _travelSections;

    public DateTime CreationDateTime { get; set; } = DateTime.Now;

    public void SetMeta(string key, string value) => _metadata[key] = value;

    public string? GetMeta(string key) => _metadata.TryGetValue(key, out var value) ? value : null;

    public bool TryGetMeta(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetValue(key, out value);

    public void ClearMeta(string key) => _metadata.Remove(key);

    public T AddSection<T>() where T : TravelSectionBase, new() {
        var section = new T();
        _travelSections.Add(section);
        return section;
    }
}