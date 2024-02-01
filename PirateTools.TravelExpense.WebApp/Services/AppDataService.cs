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
            Id = Guid.Parse("91d67031-d244-47e9-84c5-8e6f2c646507"),
            TreasurerAddress = "Piratenpartei Deutschland – Geschäftsstelle – Pflugstr. 9a – 10115 Berlin\r\nAn den Schatzmeister im Bundesvorstand (schatzmeister@piratenpartei.de)",
            TravelExpenseRegulations = [ TravelExpenseRegulation.Default ]
        };
        Federations.Add(ppde);

        Federations.Add(new Federation("LV", "Landesverband", "BW", "Baden-Württemberg") {
            Id = Guid.Parse("6ca8375d-0858-4ec9-a46d-0162883c8dfc"),
            TreasurerAddress = "Piratenpartei Baden-Württemberg - Gartenstrasse 32 - 72764 Reutlingen\r\nAn den Schatzmeister im Landesvorstand (schatzmeister@piratenpartei-bw.de)",
            Parent = ppde,
            TravelExpenseRegulations = [TravelExpenseRegulation.Default]
        });

        Federations.Add(new Federation("LV", "Landesverband", "BY", "Bayern") {
            Id = Guid.Parse("f0382493-7b20-48f2-8226-bb9f4a09a98e"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "BE", "Berlin") {
            Id = Guid.Parse("a96cffdc-f96f-43ff-a31c-8449420cedd0"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "BB", "Brandenburg") {
            Id = Guid.Parse("51fe0485-0c23-4ed8-8598-0170b133eeab"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "HB", "Bremen") {
            Id = Guid.Parse("f8e4c4f7-758f-4f38-84ce-8ab1ef78a89b"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "HH", "Hamburg") {
            Id = Guid.Parse("030f4d31-8a66-491b-a306-78dfa26d6a88"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "HE", "Hessen") {
            Id = Guid.Parse("6a06d22e-7df4-417b-9954-b6bcefaab6be"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "MV", "Mecklenburg-Vorpommern") {
            Id = Guid.Parse("bce3c4cc-9006-4dff-81ba-cc3c971d7207"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "NI", "Niedersachsen") {
            Id = Guid.Parse("db25b44c-6485-47e2-8d07-8cf187080343"),
            TreasurerAddress = "Schatzmeister Niedersachsen (schatzmeister@piraten-nds.de)",
            Parent = ppde,
            TravelExpenseRegulations = [
                new TravelExpenseRegulation() {
                    UseFile = "LVNDS.pdf",
                    AvailableFrom = new DateOnly(2022, 3, 1),
                    MotorBikeCompensation = 0.20
                }
            ]
        });

        Federations.Add(new Federation("LV", "Landesverband", "NW", "Nordrhein-Westfalen") {
            Id = Guid.Parse("71dd83f0-3fc1-4a87-bcd7-a0f2f7abf19c"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "RP", "Rheinland-Pfalz") {
            Id = Guid.Parse("23b85f86-f791-4831-8e15-bf0c678c0692"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "SL", "Saarland") {
            Id = Guid.Parse("56c2bbb1-fea0-4aa2-b34e-9c8b8c4a63a2"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "SN", "Saarland") {
            Id = Guid.Parse("4673e142-2d52-4313-8786-fd8502a38dd6"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "ST", "Sachsen-Anhalt") {
            Id = Guid.Parse("6968a596-eaa1-4c63-8513-3e0d349851aa"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "SH", "Schleswig-Holstein") {
            Id = Guid.Parse("604b3f1a-9606-48e9-ab49-53247c1930de"),
            Parent = ppde
        });

        Federations.Add(new Federation("LV", "Landesverband", "TH", "Thüringen") {
            Id = Guid.Parse("43753f21-ad43-42c2-afac-505ed14fc1ff"),
            Parent = ppde
        });
    }

    public async Task LoadDataAsync() {
        if (LoadingCompleted)
            return;

        OpfsHandle = await StorageManager.GetOriginPrivateDirectoryAsync();
        await TryLoadConfigAsync();
        FixUp();
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

            List<TravelExpenseReport>? loadedReports = null;
            try {
                loadedReports = JsonSerializer.Deserialize<List<TravelExpenseReport>>(await file.TextAsync());
            } catch (Exception) {
                if (await OpfsHandle.FileExists("reports.json"))
                    await OpfsHandle.RemoveEntryAsync("reports.json");
            }

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

        if (await OpfsHandle.FileExists("images")) {
            var imagesHandle = await OpfsHandle.GetDirectoryHandleAsync("images");
            var files = await imagesHandle.ValuesAsync();

            foreach (var imgHandle in files)
                await imagesHandle.RemoveEntryAsync(await imgHandle.GetNameAsync());

            await OpfsHandle.RemoveEntryAsync("images");
        }

        Config = new();
        Reports.Clear();
    }

    private void FixUp() {
        // Fix some users not having any Federations
        foreach (var user in Config.Users) {
            user.Federation ??= Federations[0];
        }
    }
}