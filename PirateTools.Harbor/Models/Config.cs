namespace PirateTools.Harbor.Models;

public class Config {
    public string SmtpSenderEMail { get; set; } = "";
    public string SmtpSenderName { get; set; } = "";

    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }

    public string SmtpUserName { get; set; } = "";
    public string SmtpPassword { get; set; } = "";
}