using System;
using System.Collections.Generic;
using System.Text;
using FRITeam.Swapify.Entities.Enums;

namespace FRITeam.Swapify.Backend.EmailTemplate
{
    class EmailMessages
    {

        public string Subject{ get; set; }
        public string Body { get; set; }
        public string Title
        {
            get => "Dear student";
        }

        public void setSubject(TypeOfMessage messageType)
        {
            if (messageType == TypeOfMessage.RegistrationMessage)
                Subject = "Welcome on Swapify";
            if (messageType == TypeOfMessage.ConfirmSwapMessage)
                Subject = "We found student which want to swap with you";
            if (messageType == TypeOfMessage.ForgotPasswordMessage)
                Subject = "Password change request";
        }

        public void setBody(TypeOfMessage messageType)
        {
            if (messageType == TypeOfMessage.RegistrationMessage)
            {
                Body = "Your registration was successfully, now you can edit your timetable and find other" +
                       " students who want to swap their exercise or laboratory."; //maybe later add link to confirm registration
            }
            else if (messageType == TypeOfMessage.ConfirmSwapMessage)
            {
                Body = "We found someone who want to swap with you. Please visit Swapify " +
                       " and confirm or reject this request.";
            }
            else if (messageType == TypeOfMessage.ForgotPasswordMessage)
            {
                Body = "You told us you forgot your password. If you really did, click on button below. If you didn't mean to reset" +
                       " your password, then you can ignore this e-mail.";
            }
        }

    }
}
