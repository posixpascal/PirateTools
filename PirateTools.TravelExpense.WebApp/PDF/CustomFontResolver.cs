using PdfSharpCore.Fonts;
using PirateTools.TravelExpense.WebApp.Services;
using System;

namespace PirateTools.TravelExpense.WebApp.PDF;

public class CustomFontResolver : IFontResolver {
    public string DefaultFontName => "OpenSans";

    private readonly FontService FontService;

    public CustomFontResolver(FontService fontService) {
        FontService = fontService;
    }

    public byte[] GetFont(string faceName) => FontService.GetFontData(faceName);

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) {
        Console.WriteLine($"Trying to use Font with FamilyName: {familyName}, isBold: {isBold}, isItalic: {isItalic}");

        if (isBold && isItalic) {
            return new FontResolverInfo($"{familyName}-BoldItalic.ttf");
        } else if (isBold) {
            return new FontResolverInfo($"{familyName}-Bold.ttf");
        } else if (isItalic) {
            return new FontResolverInfo($"{familyName}-Italic.ttf");
        } else {
            return new FontResolverInfo($"{familyName}-Regular.ttf");
        }
    }
}