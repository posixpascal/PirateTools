using BlazorDownloadFile;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using Blazored.Modal.Services;
using PirateTools.TravelExpense.WebApp.Components.Modals;
using System;

namespace PirateTools.TravelExpense.WebApp.Pages;

public partial class Settings {
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    [Inject]
    public required IBlazorDownloadFileService DownloadFileService { get; set; }
    [Inject]
    public required IModalService ModalService { get; set; }

    private bool HasValidImportFile;
    private ExportData? ImportData;

    protected override void OnParametersSet() {
        if (!AppData.LoadingCompleted) {
            NavigationManager.NavigateTo("/");
            return;
        }
    }

    private async Task KillData() {
        await AppData.DeleteAllData();
    }

    private async Task AllowDataStorage() {
        AppData.Config.UseLocalStorage = true;
        await AppData.SaveConfigAsync();
    }

    private async Task ImportJson() {
        if (ImportData == null)
            return;

        AppData.Config.Users = ImportData.Users;
        AppData.Reports = ImportData.Reports;

        await AppData.SaveConfigAsync();
        await AppData.SaveReports();
        NavigationManager.NavigateTo("/");
    }

    private async Task ExportJson() {
        var data = new ExportData {
            Users = AppData.Config.Users,
            Reports = AppData.Reports
        };

        var serialized = JsonSerializer.Serialize(data);
        var binary = Encoding.UTF8.GetBytes(serialized);

        await DownloadFileService.DownloadFile("reisekosten.piratenpartei.de.json",
            binary, "application/octet-stream");
    }

    private async Task OnFileChanged(InputFileChangeEventArgs e) {
        HasValidImportFile = false;

        if (e.File.ContentType != "application/json") {
            Console.WriteLine("Invalid ContentType");
            return;
        }

        IModalReference? modal = null;

        try {
            modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
                .Add(nameof(SpinnerModal.Title), "Daten werden gelesen"));

            var readStream = e.File.OpenReadStream(11_000_000);
            ImportData = await JsonSerializer.DeserializeAsync<ExportData>(readStream);
            HasValidImportFile = true;
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }

        modal?.Close();
    }

    private class ExportData {
        public List<Pirate> Users { get; set; } = [];
        public List<TravelExpenseReport> Reports { get; set; } = [];
    }
}