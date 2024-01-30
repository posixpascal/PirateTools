namespace PirateTools.Web.Common.Bootstrap;

public enum BootstrapType {
    None,
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
    Light,
    Dark
}

public static class BootstrapTyoeExtensions {
    public static string ToCssClass(this BootstrapType type) {
        return type switch {
            BootstrapType.None => "",
            BootstrapType.Primary => "primary",
            BootstrapType.Secondary => "secondary",
            BootstrapType.Success => "success",
            BootstrapType.Danger => "danger",
            BootstrapType.Warning => "warning",
            BootstrapType.Info => "info",
            BootstrapType.Light => "light",
            BootstrapType.Dark => "dark",
            _ => ""
        };
    }
}