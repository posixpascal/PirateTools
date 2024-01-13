using Microsoft.AspNetCore.Components;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Users;

public partial class UsersList {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnParametersSetAsync() {
        await AppData.LoadDataAsync();
    }

    private async Task AddNew() {
        var user = new Pirate();
        user.Id = Guid.NewGuid();
        user.Address = new();

        AppData.Config.Users.Add(user);
        await AppData.SaveConfigAsync();
        EditUser(user);
    }

    private void EditUser(Pirate user) => NavigationManager.NavigateTo($"/Users/Edit/{user.Id}");

    private async Task DeleteUser(Pirate user) {
        AppData.Config.Users.Remove(user);
        await AppData.SaveConfigAsync();
    }
}