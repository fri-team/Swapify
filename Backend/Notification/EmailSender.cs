using RestSharp;
using RestSharp.Authenticators;
using System;


namespace FRITeam.Swapify.Backend.Notification
{
    public class EmailSender : IEmailSender
    {
        private readonly IMailGunSettings _settings;
        public EmailSender(IMailGunSettings settings)
        {
            _settings = settings;
        }

        public void SendSimpleMessage(Email email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(_settings.Domain);
            client.Authenticator = new HttpBasicAuthenticator("api", _settings.ApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", _settings.SenderDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", _settings.FromEmail);
            request.AddParameter("to", email.ToEmail);
            request.AddParameter("subject", email.Subject);
            request.AddParameter("text", email.Body);
            request.Method = Method.POST;
            
            client.Execute(request);
        }
    }
}
