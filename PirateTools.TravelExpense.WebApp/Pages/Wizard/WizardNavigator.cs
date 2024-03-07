using PirateTools.Models;
using PirateTools.Models.Legacy;
using PirateTools.TravelExpense.WebApp.Models;
using System.Linq;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public static class WizardNavigator {
    public static WizardStep GetPrevious(TravelExpenseReport_V0 report, WizardStep current) {
        switch (current) {
            case WizardStep.UserSelector:
            case WizardStep.UserDataEntry:
                return WizardStep.UserSelection;
            case WizardStep.FederationDataEntry:
                return report.UsedExistingUser ? WizardStep.UserSelector : WizardStep.UserDataEntry;
            case WizardStep.ExpenseBaseDataEntry:
                return WizardStep.FederationDataEntry;
            case WizardStep.TravelTypeSelection:
                return WizardStep.ExpenseBaseDataEntry;
            case WizardStep.TravelCostsVehicle:
            case WizardStep.TravelCostsPublicTransit:
                return WizardStep.TravelTypeSelection;
            case WizardStep.NightlyCostTypeSelection:
                return report.VehicleUsed.IsPrivateVehicle()
                    ? WizardStep.TravelCostsVehicle : WizardStep.TravelCostsPublicTransit;
            case WizardStep.NightlyCostExact:
                return WizardStep.NightlyCostTypeSelection;
            case WizardStep.OtherCostsSelection:
                return report.AccommodationType == AccommodationType.Specified
                    ? WizardStep.NightlyCostExact : WizardStep.NightlyCostTypeSelection;
            case WizardStep.OtherCostsOverview:
                return WizardStep.OtherCostsSelection;
            case WizardStep.DonationSelection:
                return report.OtherCosts.Count > 0
                    ? WizardStep.OtherCostsOverview : WizardStep.OtherCostsSelection;
            case WizardStep.BankInformationEntry:
                return WizardStep.DonationSelection;
            case WizardStep.EntryIssues:
                return report.IsDonatingAll() ? WizardStep.DonationSelection : WizardStep.BankInformationEntry;
            case WizardStep.AdditionalNotesSelection:
                //var anyIssues = TravelExpenseReportValidator.ValidateReport(report).Any();

                //if (anyIssues) {
                //    return WizardStep.EntryIssues;
                //} else {
                //    return report.IsDonatingAll()
                //        ? WizardStep.DonationSelection : WizardStep.BankInformationEntry;
                //}
                return WizardStep.None;
            case WizardStep.AdditionalNotesEntry:
                return WizardStep.AdditionalNotesSelection;
            case WizardStep.Summary:
                //TODO: This needs to check for whether or not additional nodes have been added or not
                return WizardStep.AdditionalNotesSelection;
            default: return WizardStep.None;
        }
    }

    public static WizardStep GetNext(TravelExpenseReport_V0 report, WizardStep current) {
        return WizardStep.None;
    }
}