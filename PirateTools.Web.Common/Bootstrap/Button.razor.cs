using Microsoft.AspNetCore.Components;

namespace PirateTools.Web.Common.Bootstrap;

public partial class Button : ComponentBase {
    [Parameter]
    public EventCallback OnClick { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    public required string Title { get; set; }
    [Parameter]
    public BootstrapType Type { get; set; }
    [Parameter]
    public bool Small { get; set; }
}