// Services/EmailService.cs
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly EmailRepository _emailRepository;

    public EmailService()
    {
        _emailRepository = new EmailRepository();
    }

    public async Task SendDocumentEmailAsync(EmailModel emailModel)
    {
        string subject = $" {emailModel.Beneficio}";
        /*string body = $"Se ha enviado un nuevo documento para el empleado con DNI: {emailModel.Dni}. Descripción: {emailModel.Descripcion}";*/
        string body = $"Estimado Colaborador:Mediante la presente, le informamos que se acaba de cargar su boleta de pago correspondiente al presente mes al INTRANET QF. Favor de visualizar y firmar la boleta de pago.Saludos Cordiales.";





        // Envía el correo con el documento adjunto
        /*  await _emailRepository.SendEmailAsync(emailModel.Email, subject, body, emailModel.Document);*/

        await _emailRepository.SendEmailAsync(emailModel.Email, subject, body);
    }
}