using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages.TravelExpenseWizard;

public partial class UserSelection {
    private void UseUser(Pirate user) {
        Report.SetMeta(MetaKeys.PirateId, user.Id.ToString());
        Report.User = user;
        NavigationManager.NavigateTo(TravelExpenseWizardStep.FederationSelection.ToPage());
    }

    private void NewUser() {
        Report.User = new Pirate {
            Id = Guid.NewGuid(),
            Address = new Address(),
            Federation = Federation.None
        };
        NavigationManager.NavigateTo(TravelExpenseWizardStep.UserDataEntry.ToPage());
    }
}