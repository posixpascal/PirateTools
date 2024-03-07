using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;

namespace PirateTools.TravelExpense.WebApp.Components.TravelExpenseWizard;

public partial class TravelExpenseWizardHeader {
    [Parameter]
    public required RenderFragment Title { get; set; }
    [Parameter]
    public TravelExpenseWizardStep Step { get; set; }
}