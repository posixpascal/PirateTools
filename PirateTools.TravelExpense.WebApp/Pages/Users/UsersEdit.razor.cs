using Microsoft.AspNetCore.Components;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Users;

public partial class UsersEdit {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Parameter]
    public required Guid UserId { get; set; }

    private Pirate? User;

    private Guid SelectedFedeationId {
        get => User?.Federation?.Id ?? Guid.Empty;
        set {
            if (User == null)
                return;

            User.Federation = AppData.Federations.Find(f => f.Id == value)!;
        }
    }

    protected override void OnParametersSet() {
        User = AppData.Config.Users.Find(u => u.Id == UserId);

        User ??= new Pirate();
        User.Federation ??= AppData.Federations[0]!;
    }

    private async Task Save() {
        await AppData.SaveConfigAsync();
        NavigationManager.NavigateTo("/Users/List");
    }
}