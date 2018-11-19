using System;
using System.Collections.Generic;
using System.Text;


namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Email.Email email);
    }
}
