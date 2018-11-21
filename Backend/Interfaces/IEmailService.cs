using System.Net.Mail;
using FRITeam.Swapify.Backend.Enums;
using FRITeam.Swapify.Backend.Notification;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        //void SendReqistrationMail(EmailBase data, string confirmationLink);
        //void SendRestorePaswordMail(EmailBase data, string resetPasswordLink);
        void ComposeAndSendMail(EmailTypes typeOfEmail, EmailBase email, string generateLink);
        //void SendMailMessage(MailMessage message);
        //void ComposeMessage(EmailBase data);
    }
}
