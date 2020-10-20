
namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        bool SendFeedbackEmail(string userEmail, string userName, string subject, string body);
        bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType);
    }
}
