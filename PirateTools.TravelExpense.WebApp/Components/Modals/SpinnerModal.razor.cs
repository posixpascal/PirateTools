using Microsoft.AspNetCore.Components;

namespace PirateTools.TravelExpense.WebApp.Components.Modals;

public partial class SpinnerModal {
    [Parameter]
    public required string Title { get; set; }
}