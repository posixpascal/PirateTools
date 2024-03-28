using PirateTools.Harbor.Models;
using System.Net;
using System.Net.Mail;

namespace PirateTools.Harbor.Services;

public class SmtpService {
	private readonly Config _config;

	public SmtpService(Config config) {
		_config = config;
	}

	public void SendMail(string to,  string subject, string body) {
		var mm = new MailMessage();
		using var smtp = new SmtpClient();

		mm.From = new MailAddress(_config.SmtpSenderEMail, _config.SmtpSenderName);
		mm.To.Add(new MailAddress(to));
		mm.Subject = subject;
		mm.Body = body;

		smtp.Host = _config.SmtpServer;
		smtp.Port = _config.SmtpPort;
		smtp.EnableSsl = true;

        var credentials = new NetworkCredential {
            UserName = _config.SmtpUserName,
            Password = _config.SmtpPassword
        };

		smtp.Credentials = credentials;
		smtp.Send(mm);
	}
}