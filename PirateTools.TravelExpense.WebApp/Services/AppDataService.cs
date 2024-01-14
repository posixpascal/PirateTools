using KristofferStrube.Blazor.FileSystem;
using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.Utility;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Services;

public class AppDataService {
    public bool LoadingCompleted { get; private set; }

    public TravelExpenseReport? CurrentReport { get; set; }
    public WizardStep CurrentStep { get; set; }

    public List<TravelExpenseReport> Reports { get; set; } = [];

    public AppConfig Config { get; set; } = new();

    public List<Federation> Federations = [];

    private readonly IStorageManagerService StorageManager;

    private FileSystemDirectoryHandle? OpfsHandle;

    public AppDataService(IStorageManagerService storageManager) {
        StorageManager = storageManager;

        var ppde = new Federation("PP", "Piratenpartei", "DE", "Deutschland") {
            Id = Guid.Parse("91d67031-d244-47e9-84c5-8e6f2c646507")
        };

        var lvnds = new Federation("LV", "Landesverband", "NDS", "Niedersachsen") {
            Id = Guid.Parse("db25b44c-6485-47e2-8d07-8cf187080343"),
            TreasurerAddress = "Schatzmeister Niedersachsen (schatzmeister@piraten-nds.de)",
            Parent = ppde
        };

        Federations.Add(ppde);
        Federations.Add(lvnds);
    }

    public async Task LoadDataAsync() {
        if (LoadingCompleted)
            return;

        OpfsHandle = await StorageManager.GetOriginPrivateDirectoryAsync();
        await TryLoadConfigAsync();
        LoadingCompleted = true;
    }

    private async Task TryLoadConfigAsync() {
        if (OpfsHandle == null) {
            Console.WriteLine("TryLoadConfigAsync: OpfsHandle is null");
            return;
        }

        if (await OpfsHandle.FileExists("config.json")) {
            var configHandle = await OpfsHandle.GetFileHandleAsync("config.json");
            var file = await configHandle.GetFileAsync();
            var loadedConfig = JsonSerializer.Deserialize<AppConfig>(await file.TextAsync());

            if (loadedConfig != null)
                Config = loadedConfig;
        }

        if (await OpfsHandle.FileExists("reports.json")) {
            var reportsHandle = await OpfsHandle.GetFileHandleAsync("reports.json");
            var file = await reportsHandle.GetFileAsync();
            var loadedReports = JsonSerializer.Deserialize<List<TravelExpenseReport>>(await file.TextAsync());

            if (loadedReports != null)
                Reports = loadedReports;
        }
    }

    public async Task SaveConfigAsync() {
        if (!Config.UseLocalStorage || OpfsHandle == null)
            return;

        if (await OpfsHandle.FileExists("config.json"))
            await OpfsHandle.RemoveEntryAsync("config.json");

        var configHandle = await OpfsHandle.GetFileHandleAsync("config.json",
            new FileSystemGetFileOptions {
                Create = true
            });

        var writeHandle = await configHandle.CreateWritableAsync();
        await writeHandle.WriteAsync(JsonSerializer.Serialize(Config));
        await writeHandle.CloseAsync();
        Console.WriteLine("Config Saved");
    }

    public async Task SaveReports() {
        if (!Config.UseLocalStorage || OpfsHandle == null)
            return;

        if (await OpfsHandle.FileExists("reports.json"))
            await OpfsHandle.RemoveEntryAsync("reports.json");

        var reportsHandle = await OpfsHandle.GetFileHandleAsync("reports.json",
            new FileSystemGetFileOptions {
                Create = true
            });
        var writeHandle = await reportsHandle.CreateWritableAsync();
        await writeHandle.WriteAsync(JsonSerializer.Serialize(Reports));
        await writeHandle.CloseAsync();
        Console.WriteLine("Reports Saved");
    }

    public async Task DeleteAllData() {
        if (OpfsHandle == null)
            return;

        await OpfsHandle.RemoveEntryAsync("config.json");

        if (await OpfsHandle.FileExists("reports.json"))
            await OpfsHandle.RemoveEntryAsync("reports.json");

        if (await OpfsHandle.FileExists("images"))
            await OpfsHandle.RemoveEntryAsync("images");

        Config = new();
        Reports.Clear();
    }
}