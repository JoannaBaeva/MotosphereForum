using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var smtpServer = _configuration["EmailSettings:SMTPServer"];
        var smtpPortString = _configuration["EmailSettings:SMTPPort"];
        var smtpUsername = _configuration["EmailSettings:SMTPUsername"];
        var smtpPassword = _configuration["EmailSettings:SMTPPassword"];
        var enableSSLString = _configuration["EmailSettings:EnableSSL"];

        // Ensure none of the required settings are null
        if (string.IsNullOrEmpty(smtpServer) ||
            string.IsNullOrEmpty(smtpPortString) ||
            string.IsNullOrEmpty(smtpUsername) ||
            string.IsNullOrEmpty(smtpPassword) ||
            string.IsNullOrEmpty(enableSSLString))
        {
            throw new Exception("SMTP settings are missing from configuration.");
        }

        if (!int.TryParse(smtpPortString, out int smtpPort))
        {
            throw new Exception("Invalid SMTP port format.");
        }

        if (!bool.TryParse(enableSSLString, out bool enableSSL))
        {
            enableSSL = true; // Default to true if parsing fails
        }

        var smtpClient = new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = enableSSL
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUsername),
            Subject = subject,
            IsBodyHtml = true,
            Body = message
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
