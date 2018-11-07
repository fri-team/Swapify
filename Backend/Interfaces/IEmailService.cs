using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        void SendRegistrationConfirmationEmail(string receiverEmail, string confirmationLink);
        void SendResetPasswordEmail(string receiverEmail, string resetLink);
    }
}
