// Repositories/EmailRepository.cs
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailRepository
{
    private readonly string _smtpServer = "mail.qf.com.pe"; // Cambia según tu proveedor
    private readonly int _smtpPort = 587; // Cambia según tu proveedor
    private readonly string _fromEmail = "lvelasquez@qf.com.pe"; // Tu correo
    private readonly string _fromPassword = "Luis@2023$";

    public async Task SendEmailAsync(string toEmail, string subject, string body, IFormFile file)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
            client.EnableSsl = true;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_fromEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = false;
                mailMessage.To.Add(toEmail);

                // Si hay un archivo, se agrega como adjunto
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(stream.ToArray()), fileName));
                    }
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}