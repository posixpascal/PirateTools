using PirateTools.Models;
using PirateTools.TravelExpense.WebApp.Models;
using System;
using System.Collections.Generic;

namespace PirateTools.TravelExpense.WebApp.Services;

public class AppDataService {
    public TravelExpenseReport? CurrentReport { get; set; }
    public WizardStep CurrentStep { get; set; }

    public List<Federation> Federations = [];

    public AppDataService() {
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
}