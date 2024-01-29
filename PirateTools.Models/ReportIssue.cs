using System;

namespace PirateTools.Models;

public class ReportIssue {
    public ReportIssueSeverity Severity { get; init; }
    public required string Message { get; init; }
    public string? Step { get; init; }

    public static ReportIssue Error(string message, string? step = null) => new() {
        Severity = ReportIssueSeverity.Error,
        Message = message,
        Step = step
    };

    public static ReportIssue Warning(string message, string? step = null) => new() {
        Severity = ReportIssueSeverity.Warning,
        Message = message,
        Step = step
    };

    public static ReportIssue Info(string message, string? step = null) => new() {
        Severity = ReportIssueSeverity.Info,
        Message = message,
        Step = step
    };
}

public enum ReportIssueSeverity {
    Info,
    Warning,
    Error
}

public static class ReportIssueSeverityExtensions {
    public static string ToBootstrap(this ReportIssueSeverity severity) {
        return severity switch {
            ReportIssueSeverity.Info => "info",
            ReportIssueSeverity.Warning => "warning",
            ReportIssueSeverity.Error => "danger",
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }

    public static string ToHeading(this ReportIssueSeverity severity) {
        return severity switch {
            ReportIssueSeverity.Info => "Hinweis",
            ReportIssueSeverity.Warning => "Warnung",
            ReportIssueSeverity.Error => "Problem",
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }
}