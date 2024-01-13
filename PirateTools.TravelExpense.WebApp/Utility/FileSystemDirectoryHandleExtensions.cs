using KristofferStrube.Blazor.FileSystem;
using System.Threading.Tasks;

namespace PirateTools.TravelExpense.WebApp.Utility;

public static class FileSystemDirectoryHandleExtensions {
    public static async Task<bool> FileExists(this FileSystemDirectoryHandle handle, string filename) {
        foreach (var child in await handle.ValuesAsync()) {
            if (await child.GetNameAsync() == filename)
                return true;
        }

        return false;
    }
}