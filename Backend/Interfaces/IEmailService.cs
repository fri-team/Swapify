using System.Net.Mail;
using FRITeam.Swapify.Backend.Enums;
using FRITeam.Swapify.Backend.Notification;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        void ComposeAndSendMail(EmailTypes typeOfEmail, EmailBase email, string generateLink);
    }
}
