using Microsoft.Extensions.Logging;

namespace FRITeam.Swapify.SwapifyBase.Emails
{
    class ConfirmExchangeEmail : ConfirmationEmailBase
    {
        protected override string Subject => "Swapify výmena";
        protected override string PathToTemplate => @"Emails/EmailTemplates/ConfirmationExchangeTemplate.html";

        public ConfirmExchangeEmail(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl, string confirmationLink)
            : base(loggerFactory, sender, senderDisplayName, receiver, baseUrl, confirmationLink)
        {
        }
    }
}
