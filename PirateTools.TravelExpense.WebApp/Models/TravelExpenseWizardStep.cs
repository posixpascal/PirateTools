namespace PirateTools.TravelExpense.WebApp.Models;

public enum TravelExpenseWizardStep {
    None,
    UserSelection, // List of existing user + field to just enter data
    UserDataEntry, // If data entry is required
    FederationSelection, // Which Federation was traveled for
    TripDataEntry, // Reason, Resolution, Dates, Times, Destination
    TripBuilder, // TravelSection UI/Builder
    BankInformationEntry,
    EntryIssues,
    Summary // Including Additional notes field
}

public static class TravelExpenseWizardStepExtensions {
    public static string ToPage(this TravelExpenseWizardStep step) {
        return "/TravelExpenseWizard/" + step switch {
            TravelExpenseWizardStep.UserSelection => "UserSelection",
            TravelExpenseWizardStep.UserDataEntry => "UserDataEntry",
            TravelExpenseWizardStep.FederationSelection => "FederationSelection",
            TravelExpenseWizardStep.TripDataEntry => "TripDataEntry",
            TravelExpenseWizardStep.TripBuilder => "TripBuilder",
            TravelExpenseWizardStep.BankInformationEntry => "BankInformationEntry",
            TravelExpenseWizardStep.EntryIssues => "EntryIssues",
            TravelExpenseWizardStep.Summary => "Summary",
            _ => "/",
        };
    }
}