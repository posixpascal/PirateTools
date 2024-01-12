using Microsoft.AspNetCore.Components;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.TravelExpense.WebApp.Models;
using PirateTools.TravelExpense.WebApp.PDF;
using PdfSharpCore.Fonts;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Pages.Wizard;

public partial class StepUserSelection {
    [Inject]
    public required AppDataService AppData { get; set; }

    [Inject]
    public required FontService FontService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            GlobalFontSettings.FontResolver = new CustomFontResolver(FontService);

            await FontService.LoadFontAsync("OpenSans-Regular.ttf");
            await FontService.LoadFontAsync("OpenSans-Bold.ttf");
            await FontService.LoadFontAsync("OpenSans-Italic.ttf");
            await FontService.LoadFontAsync("OpenSans-BoldItalic.ttf");
        }

        if (AppData.CurrentReport == null)
            AppData.CurrentReport = new();

        AppData.CurrentStep = WizardStep.UserSelection;
    }
}