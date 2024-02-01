using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.Web.Common.BootstrapTimeline;

namespace PirateTools.TravelExpense.WebApp.Layout;

public partial class WizardSidebarOverviewLayout {
    [Inject]
    public required AppDataService AppData { get; set; }

    private TimelineItem.ItemType ItemTypeForStep(WizardStep step) {
        if (AppData.CurrentStep == step)
            return TimelineItem.ItemType.Current;

        if (AppData.CurrentStep > step)
            return TimelineItem.ItemType.Done;

        return TimelineItem.ItemType.Todo;
    }
}