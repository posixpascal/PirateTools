using System;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.TravelExpenseWizard;

public partial class FederationSelection {
    private Guid SelectedFedeationId {
        get => Report.Federation?.Id ?? Guid.Empty;
        set {
            if (Report == null)
                return;

            Report.Federation = AppData.Federations.Find(f => f.Id == value);
        }
    }

    protected override async Task OnParametersSetAsync() {
        await base.OnParametersSetAsync();

        if (Report == null)
            return;

        if (Report.Federation == null) {
            if (Report.User?.Federation != null) {
                Report.Federation = Report.User.Federation;
            } else {
                Report.Federation = AppData.Federations[0];
            }
        }
    }
}