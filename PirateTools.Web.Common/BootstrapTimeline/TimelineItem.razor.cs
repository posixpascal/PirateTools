using Microsoft.AspNetCore.Components;

namespace PirateTools.Web.Common.BootstrapTimeline;

public partial class TimelineItem {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    public EventCallback OnClick { get; set; }
    [Parameter]
    public ItemType Type { get; set; }
    [Parameter]
    public bool IsLast { get; set; }

    private string CircleBorderColor() {
        return Type switch {
            ItemType.Done => "var(--bs-success)",
            ItemType.Current => "var(--bs-primary)",
            ItemType.Todo => "var(--bs-primary-text-emphasis)",
            _ => ""
        };
    }

    public enum ItemType {
        Done,
        Current,
        Todo
    }
}