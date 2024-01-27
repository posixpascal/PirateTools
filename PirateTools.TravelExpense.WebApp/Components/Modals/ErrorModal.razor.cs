using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Components.Modals;

public partial class ErrorModal {
    [CascadingParameter]
    public required BlazoredModalInstance Modal { get; set; }

    [Parameter]
    public required string Title { get; set; }
    [Parameter]
    public string? Content { get; set; }

    private async Task Close() => await Modal.CloseAsync();
}