namespace PirateTools.TravelExpense.WebApp.Models;

public enum WizardStep {
    None,
    UserSelection,
    UserDataEntry,
    FederationDataEntry,
    ExpenseBaseDataEntry, // Reason, Resolution, Dates, Times, Location
    TravelTypeSelection, // Car, Motorbike, Public Transit
    TravelCostsVehicle,
    TravelCostsPublicTransit,
    NightlyCostTypeSelection,
    NightlyCostExact,
    OtherCostsSelection,
    OtherCostsOverview,
    DonationSelection, // Donate Amount Input, Checkbox "Donate All"
    BankInformationEntry,
    Summary
}