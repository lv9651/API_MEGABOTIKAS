using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using SISLAB_API.Areas.Maestros.Models;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }
    public async Task<bool> SendConfirmationEmailAsync(string toEmail, string toName)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = "Confirmación de Registro - Farmacia QF";
            message.Body = new TextPart("html")
            {
                Text = $"<h2>Hola {toName}!</h2><p>Gracias por registrarte en Farmacia QF. Tu registro se ha completado correctamente.</p>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando correo: {ex.Message}");
            return false;
        }
    }

}