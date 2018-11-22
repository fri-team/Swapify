
namespace FRITeam.Swapify.Backend.Emails.ConfirmationEmails
{
    public class RestorePasswordEmail : ConfirmationEmailBase
    {
        public override string Subject => "Zabudnuté heslo na Swapify";
        protected override string PathToTemplate => @"Emails/EmailTemplates/RestorePasswordTemplate.html";

        public RestorePasswordEmail(string sender, string senderDisplayName, string receiver)
            : base(sender, senderDisplayName, receiver)
        {
        }
    }
}
