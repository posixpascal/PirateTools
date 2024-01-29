using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Components.Modals;

public partial class DeleteConfirmModal {
    [CascadingParameter]
    public required BlazoredModalInstance Modal { get; set; }

    [Parameter]
    public required string Item { get; set; }
    [Parameter]
    public string? CustomMessage { get; set; }

    private async Task Close() => await Modal.CancelAsync();
    private async Task Confirm() => await Modal.CloseAsync(ModalResult.Ok(true));
}