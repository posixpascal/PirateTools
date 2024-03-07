using System.Diagnostics.CodeAnalysis;

namespace PirateTools.Models;

public class FileReference {
    [MemberNotNullWhen(true, nameof(FileName))]
    public bool IsSet => FileName != null;

    public string? FileName { get; set; }

    public FileReference Clone() {
        return new FileReference() {
            FileName = FileName
        };
    }
}