using Microsoft.AspNetCore.Components;

namespace PirateTools.Web.Common.BootstrapTimeline;

public partial class Timeline {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}