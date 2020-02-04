
namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        bool SendFeedbackEmail(string receiver, string context);
        bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType);
    }
}
