using PdfSharpCore.Fonts;
using PirateTools.TravelExpense.WebApp.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Services;

public class FontService {
    private readonly HttpClient Http;

	private readonly Dictionary<string, byte[]> FontCahce = [];

	public FontService(HttpClient http) {
		Http = http;
        GlobalFontSettings.FontResolver = new CustomFontResolver(this);
    }

	public byte[] GetFontData(string fontName) {
		if (!FontCahce.TryGetValue(fontName, out var data)) {
			Console.WriteLine($"Could not find Font: {fontName}");
            return [];
        }

        return data;
	}

	public async Task LoadFontAsync(string name) {
		if (FontCahce.ContainsKey(name))
			return;

		var data = await DownloadFontDataAsync(name);
		Console.WriteLine($"Cached Font with name: {name}");
		FontCahce.Add(name, data);
	}

	private async Task<byte[]> DownloadFontDataAsync(string name) {
		var fontStream = await Http.GetStreamAsync($"Resources/Fonts/{name}");
        await using var memoryStream = new MemoryStream();
		await fontStream.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
    }
}