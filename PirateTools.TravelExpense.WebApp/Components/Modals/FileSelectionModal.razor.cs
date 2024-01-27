using Blazored.Modal;
using Blazored.Modal.Services;
using ClipLazor.Components;
using Humanizer;
using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PirateTools.TravelExpense.WebApp.Services;
using PirateTools.TravelExpense.WebApp.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClipLazor.Enums;
using System;
using System.IO;

namespace PirateTools.TravelExpense.WebApp.Components.Modals;

public partial class FileSelectionModal {
    [CascadingParameter]
    public required BlazoredModalInstance Modal { get; set; }

    [Inject]
    public required IStorageManagerService StorageManager { get; set; }
    [Inject]
    public required AppDataService AppData { get; set; }
    [Inject]
    public required IModalService ModalService { get; set; }
    [Inject]
    public required IClipLazor Clipboard { get; set; }

    [Parameter]
    public required string Title { get; set; }
    [Parameter]
    public string Path { get; set; } = "";
    [Parameter]
    public long MaxFileSize { get; set; } = 4_194_304;

    private readonly List<ImageInfo> Files = [];

    private bool Loading = true;
    private bool AddNew;

    private bool CanUseClipboard;

    private async Task Cancel() {
        if (AddNew) {
            AddNew = false;
            return;
        }

        Loading = true;
        StateHasChanged();
        await Modal.CloseAsync();
    }

    private async Task Select(string fileName) {
        Loading = true;
        StateHasChanged();
        await Modal.CloseAsync(ModalResult.Ok(fileName));
    }

    private void AddNewClicked() => AddNew = true;

    private async Task OnFileChanged(InputFileChangeEventArgs e) {
        if (e.File.Size > MaxFileSize) {
            ModalService.Show<ErrorModal>("", new ModalParameters()
                .Add(nameof(ErrorModal.Title), "Datei zu Groß!")
                .Add(nameof(ErrorModal.Content), @$"Die ausgewählte Datei hat eine Größe von 
{e.File.Size.Bytes().Humanize()}, es können aber nur Bilder bis maximal {MaxFileSize.Bytes().Humanize()}
gespeichert werden.<br />
Bitte wähle ein kleineres Bild aus!"));
            return;
        }

        var modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
            .Add(nameof(SpinnerModal.Title), "Bild wird gespeichert"));
        var opfsHandle = await StorageManager.GetOriginPrivateDirectoryAsync();
        var imgDirHandle = await opfsHandle.GetDirectoryHandleAsync("images",
            StorageUtility.DefaultDirOptions);

        Loading = true;
        await imgDirHandle.StoreFile(e.File.Name, e.File.OpenReadStream(MaxFileSize));
        modal.Close();
        await Reload();
        AddNew = false;
    }

    private async Task StoreFileFromClipboard() {
        if (!CanUseClipboard)
            return;

        Loading = true;

        var data = await Clipboard.ReadDataAsync("image/png");
        if (data.IsEmpty) {
            ModalService.Show<ErrorModal>("", new ModalParameters()
                .Add(nameof(ErrorModal.Title), "Kein Bild!")
                .Add(nameof(ErrorModal.Content), @"Die Daten in der Zwischenablage konnten nicht als
PNG gelesen werden!"));
            return;
        }

        if (data.Length > MaxFileSize) {
            ModalService.Show<ErrorModal>("", new ModalParameters()
                .Add(nameof(ErrorModal.Title), "Datei zu Groß!")
                .Add(nameof(ErrorModal.Content), @$"Die ausgewählte Datei hat eine Größe von 
{data.Length.Bytes().Humanize()}, es können aber nur Bilder bis maximal {MaxFileSize.Bytes().Humanize()}
gespeichert werden.<br />
Bitte wähle ein kleineres Bild aus!"));
            return;
        }

        var modal = ModalService.Show<SpinnerModal>("", new ModalParameters()
            .Add(nameof(SpinnerModal.Title), "Bild wird gespeichert"));
        var opfsHandle = await StorageManager.GetOriginPrivateDirectoryAsync();
        var imgDirHandle = await opfsHandle.GetDirectoryHandleAsync("images",
            StorageUtility.DefaultDirOptions);

        await using var memStream = new MemoryStream(data.ToArray());
        await imgDirHandle.StoreFile(Guid.NewGuid().ToString() + ".png", memStream);

        modal.Close();
        await Reload();
    }

    protected override async Task OnParametersSetAsync() {
        var clipboardSupported = await Clipboard.IsClipboardSupported();
        var clipboardReadAllowed = await Clipboard.IsPermitted(PermissionCommand.Read);

        Console.WriteLine($"Clipboard Supported: {clipboardSupported}, ClipboardReadAllowed: {clipboardReadAllowed}");

        CanUseClipboard = clipboardSupported && clipboardReadAllowed;

        await Reload();
    }

    private async Task Reload() {
        Loading = true;
        Files.Clear();

        await foreach (var f in IterateFiles()) {
            var fileName = await f.GetNameAsync();
            var file = await f.GetFileAsync();
            var blob = await file.SliceAsync();
            var data = await blob.ArrayBufferAsync();
            Files.Add(new(fileName, data));
        }

        Loading = false;
    }

    private async IAsyncEnumerable<FileSystemFileHandle> IterateFiles() {
        var dir = await StorageManager.GetOriginPrivateDirectoryAsync();
        if (Path != "") {
            if (await dir.FileExists(Path)) {
                dir = await dir.GetDirectoryHandleAsync(Path);
            } else {
                yield break;
            }
        }

        foreach (var f in await dir.ValuesAsync()) {
            if (f is not FileSystemFileHandle fileHandle)
                continue;

            yield return fileHandle;
        }
    }

    private class ImageInfo {
        public string Name { get; }
        public byte[] Data { get; }

        public ImageInfo(string name, byte[] data) {
            Name = name;
            Data = data;
        }
    }
}