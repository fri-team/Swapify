using System;
using System.IO;
using FRITeam.Swapify.Backend.Notification;
using FRITeam.Swapify.Entities.Enums;

namespace FRITeam.Swapify.Backend.EmailTemplate
{
    class EmailMessages
    {

        public Email Mail { get; set; }


        public void SetSubject(TypeOfMessage messageType)
        {
            if (messageType == TypeOfMessage.RegistrationMessage)
                Mail.Subject = "Welcome on Swapify";
            else if (messageType == TypeOfMessage.ConfirmSwapMessage)
                Mail.Subject = "We found student which want to swap with you";
            else if (messageType == TypeOfMessage.ForgotPasswordMessage)
                Mail.Subject = "Password change request";
            else if (messageType == TypeOfMessage.CreateSwapRequest)
                Mail.Subject = "Swap request";
        }

        public void SetBody(TypeOfMessage messageType)
        {
            using (StreamReader reader = new StreamReader("EmailTemplate.html"))
            {
                Mail.Body = reader.ReadToEnd();
            }

            string EmailBody = String.Empty;

            if (messageType == TypeOfMessage.RegistrationMessage)
            {
                EmailBody = "Your registration was successful, now you can edit your timetable and find other" +
                            " students who want to swap their exercise or laboratory."; //maybe later add link to confirm registration
            }
            else if (messageType == TypeOfMessage.ConfirmSwapMessage)
            {
                EmailBody = "We found someone who want to swap with you. Please visit Swapify " +
                            " and confirm or reject this request.";
            }
            else if (messageType == TypeOfMessage.ForgotPasswordMessage)
            {
                EmailBody =
                    "You told us you forgot your password. If you really did, click on button below. If you didn't mean to reset" +
                    " your password, then you can ignore this e-mail.";
            }
            else if (messageType == TypeOfMessage.CreateSwapRequest)
            {
                EmailBody = "Your swap request was registered. We will be contacting you once we find someone to swap" +
                            "you with.";
            }

            Mail.Body = Mail.Body.Replace("{body}", EmailBody);

        }
    }
}
