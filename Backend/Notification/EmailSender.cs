using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;

namespace FRITeam.Swapify.Backend.Notification
{
    public class EmailSender
    {
        public static void SendSimpleMessage(Email email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                new HttpBasicAuthenticator("api", "9f281906fd37aa0064541e8c3b7a6e8b-4836d8f5-3f14bcda");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox260b1e74a61b4b7faf43e42dd656e3a0.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", email.FromEmail);
            request.AddParameter("to", email.ToEmail);
            request.AddParameter("subject", email.Subject);
            request.AddParameter("text", email.Body);
            request.Method = Method.POST;
            
           client.Execute(request);
        }
    }
}
