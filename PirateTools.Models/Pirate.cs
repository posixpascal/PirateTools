using System;
using System.Text.Json.Serialization;

namespace PirateTools.Models;

public class Pirate {
    //Application Internal
    public Guid Id { get; set; }

    //Normal Data
    public int MemberID { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public Federation Federation { get; set; }

    public string? EMail { get; set; }
    public string? IBAN { get; set; }
    public string? BIC { get; set; }

    [JsonConstructor]
    public Pirate() { }

    public Pirate(int memberID, string name, Address address, Federation federation) {
        MemberID = memberID;
        Name = name;
        Address = address;
        Federation = federation;
    }
}
