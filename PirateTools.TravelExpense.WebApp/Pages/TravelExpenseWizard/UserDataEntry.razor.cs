using PirateTools.Models;
using System;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.TravelExpenseWizard;

public partial class UserDataEntry {
    private Guid SelectedFedeationId {
        get => Report.User?.Federation?.Id ?? Guid.Empty;
        set {
            if (Report.User == null)
                return;

            Report.User.Federation = AppData.Federations.Find(f => f.Id == value)!;
        }
    }

    protected override async Task OnParametersSetAsync() {
        await base.OnParametersSetAsync();

        if (Report == null)
            return;

        Report.User ??= new() { Id = Guid.NewGuid() };

        if (Report.User.Address == null)
            Report.User.Address = new();

        if (Report.User.Federation == null)
            Report.User.Federation = Federation.None;

        Report.ClearMeta(MetaKeys.PirateId);
    }
}