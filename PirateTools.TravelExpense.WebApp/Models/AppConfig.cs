using PirateTools.Models;
using System.Collections.Generic;

namespace PirateTools.TravelExpense.WebApp.Models;

public class AppConfig {
    public bool UseLocalStorage { get; set; }

    public List<Pirate> Users { get; set; } = [];
}