using KristofferStrube.Blazor.FileSystem;
using System.IO;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Utility;

public static class StorageUtility {
    public static readonly FileSystemGetFileOptions DefaultFileOptions = new() { Create = true };
    public static readonly FileSystemGetDirectoryOptions DefaultDirOptions = new() { Create = true };
}

public static class FileSystemDirectoryHandleExtensions {
    public static async Task<bool> FileExists(this FileSystemDirectoryHandle handle, string filename) {
        foreach (var child in await handle.ValuesAsync()) {
            if (await child.GetNameAsync() == filename)
                return true;
        }

        return false;
    }

    public static async Task StoreFile(this FileSystemDirectoryHandle handle, string filename, Stream s) {
        if (await handle.FileExists(filename))
            await handle.RemoveEntryAsync(filename);

        var fileHandle = await handle.GetFileHandleAsync(filename, StorageUtility.DefaultFileOptions);
        var writeStream = await fileHandle.CreateWritableAsync();
        await s.CopyToAsync(writeStream);
    }

    public static async Task<Stream> LoadFile(this FileSystemDirectoryHandle handle, string filename) {
        if (!await handle.FileExists(filename))
            return Stream.Null;

        var fileHandle = await handle.GetFileHandleAsync(filename, StorageUtility.DefaultFileOptions);
        var file = await fileHandle.GetFileAsync();
        return await file.StreamAsync();
    }
}