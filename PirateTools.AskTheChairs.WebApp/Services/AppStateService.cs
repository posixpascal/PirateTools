using PirateTools.Models.AskYourChairs;
using System.Collections.Generic;

namespace PirateTools.AskTheChairs.WebApp.Services;

public class AppStateService {
    public string? Token { get; set; }
    public int ActionsLeft { get; set; }

    public List<Question>? Questions { get; set; }
}