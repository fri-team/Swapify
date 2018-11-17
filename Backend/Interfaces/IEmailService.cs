using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using FRITeam.Swapify.Backend.Notification;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        void SendReqistrationMail(Email data, string confirmationLink);
        void SendRestorePaswordMail(Email data, string resetPasswordLink);
        void SendMailMessage(MailMessage message);
    }
}
