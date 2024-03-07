using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Models;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepNightlyCostTypeSelection {
    protected override void OnParametersSet() {
        //AppData.CurrentStep = WizardStep.NightlyCostTypeSelection;
    }

    private void GoBack() {
        if (AppData.CurrentReport == null)
            return;

        //if (AppData.CurrentReport.VehicleUsed == Vehicle.PublicTransit) {
            NavigationManager.NavigateTo("/StepTravelCostsPublicTransit");
        //} else if (AppData.CurrentReport.VehicleUsed == Vehicle.Undefined) {
        //    NavigationManager.NavigateTo("/StepTravelTypeSelection");
        //} else {
        //    NavigationManager.NavigateTo("/StepTravelCostsVehicle/" + (int)AppData.CurrentReport.VehicleUsed);
        //}
    }

    private void TypeSpecified() => SelectType(AccommodationType.Specified, "/StepNightlyCostExact");

    private void TypeFlatRate() => SelectType(AccommodationType.FlatRate, "/StepOtherCostsSelection");

    private void TypeNone() => SelectType(AccommodationType.None, "/StepOtherCostsSelection");

    private void SelectType(AccommodationType type, string navRoute) {
        if (AppData.CurrentReport == null)
            return;

        //AppData.CurrentReport.AccommodationType = type;
        NavigationManager.NavigateTo(navRoute);
    }
}