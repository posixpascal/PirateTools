namespace PirateTools.Models;

public class Address {
    /// <summary>Street or Block</summary>
    public string LocalIdentifier { get; set; }

    /// <summary>HouseNbr and any addition (e.g. "a")</summary>
    public string HouseIdentifier { get; set; }

    public string PostCode { get; set; }

    public string City { get; set; }

    public Address() {
        LocalIdentifier = "";
        HouseIdentifier = "";
        PostCode = "";
        City = "";
    }

    public Address(string localIdentifier, string houseIdentifier, string postCode, string city) {
        LocalIdentifier = localIdentifier;
        HouseIdentifier = houseIdentifier;
        PostCode = postCode;
        City = city;
    }

    public bool IsValidAddress() {
        if (string.IsNullOrEmpty(LocalIdentifier))
            return false;
        if (string.IsNullOrEmpty(HouseIdentifier))
            return false;

        if (string.IsNullOrEmpty(PostCode))
            return false;
        if (string.IsNullOrEmpty(City))
            return false;

        return true;
    }
}