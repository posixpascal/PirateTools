using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Components;

public partial class BootstrapModal {
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public bool ShowCloseX { get; set; } = true;
    [Parameter]
    public RenderFragment? Body { get; set; }
    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public int MinWidth { get; set; } = 33;

    [CascadingParameter]
    public required BlazoredModalInstance BlazoredModal { get; set; }

    private async Task CloseX() {
        await BlazoredModal.CloseAsync();
    }
}