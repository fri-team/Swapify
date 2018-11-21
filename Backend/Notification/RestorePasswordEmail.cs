using System.IO;
using Mustache;

namespace FRITeam.Swapify.Backend.Notification
{
    public class RestorePasswordEmail : EmailBase
    {
        private const string PathToTemplate = "EmailTemplate/RestorePasswordTemplate.html";
        private const string ConstImg1 = "cid:mac";
        private const string ConstImg2 = "cid:swapify";

        public RestorePasswordEmail()
        {
        }

        public RestorePasswordEmail(EmailBase baseEmail)
            : base()
        {

        }

        public string GetSubject()
        {
            return Subject = "Zabudnuté heslo";
        }

        public string GetBody(string baseUrl, string resetPasswordLink)
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
                restorePasswordLink = resetPasswordLink,
                img1 = ConstImg1,
                img2 = ConstImg2
            });
            return Body;
        }
    }
}
