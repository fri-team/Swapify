using System.IO;
using Mustache;

namespace FRITeam.Swapify.Backend.Notification
{
    public class RegistrationEmail : EmailBase
    {
        private const string PathToTemplate = "EmailTemplate/RegistrationTemplate.html";
        private const string ConstImg1 = "cid:mac";
        private const string ConstImg2 = "cid:swapify";

        public RegistrationEmail()
        {
        }

        public RegistrationEmail(EmailBase baseEmail)
            : base()
        {
            
        }

        public string GetSubject()
        {
            return Subject = "Registrácia na SWAPIFY";
        }

        public string GetBody(string baseUrl, string confirmLink)
        {
            using (var reader = new StreamReader(PathToTemplate))
            {
                Body = reader.ReadToEnd();
            }

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(Body);
            Body = generator.Render(new
            {
                link = baseUrl,
                confirmationLink = confirmLink,
                img1 = ConstImg1,
                img2 = ConstImg2
            });
            return Body;
        }
    }
}
