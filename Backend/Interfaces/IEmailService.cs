
namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType);
        void SendResetPasswordEmail(string receiverEmail, string resetLink);
    }
}
