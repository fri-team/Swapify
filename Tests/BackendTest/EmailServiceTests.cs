using System.IO;
using FRITeam.Swapify.Backend.Settings;
using Mustache;
using Xunit;

namespace BackendTest
{
    public class EmailServiceTests
    {
        [Fact]
        public void MustacheFillEnvVarToTemplate()
        {
            var path = "EmailTemplate/RegistrationTemplate.html";
            string body;
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(body);
            var url = new UrlSettings() { DevelopUrl = "http://localhost:3000/" };

            var html = generator.Render(new
            {
                link =  url.DevelopUrl,
                img1 = "cid:Mac",
                img2 = "cid:Swapify",
                confirmationLink = "skuska"
            });
            Assert.Equal("<!DOCTYPE html><html lang=\"svk\"><head>  <title>Registration template</title></head><body>  <table id=\"outer_tbl\" style=\"width: 100%; font-family: Roboto; font-size: 22px; font-weight: 300; border: 0; text-align: center; display: inline-table\">    <tr>      <td style=\"width: 100%;height: 100%; background-color: white\">        <table id=\"inner_tbl\" style=\"max-width: 650px; border-spacing: 0; background-color: whitesmoke; display: inline-table\">          <tr style=\"background-color: darkred\">            <td>              <img id=\"mac_img\" src=\"cid:Mac\" alt=\"mac_pic\" style=\"display: block; width: 100%; height: auto;\">            </td>            <td style=\"border-collapse:collapse;color:#000000;padding:0;vertical-align:top;width:75%\">              <img id=\"logo_img\" src=\"cid:Swapify\" alt=\"Logo swapify\" tabindex=\"0\" style=\"display: block; width: 100%; height: 37%;\">            </td>          </tr>          <tbody style=\"background-color: whitesmoke\">          <tr>            <td colspan=\"2\">              <h2></h2>            </td>          </tr>          <tr>            <td colspan=\"2\"><h2>Registrácia prebehla úspešne</h2></td>          </tr>          <tr class=\"spacer\" style=\"padding: 20px; display: block\"><td colspan=\"2\"></td></tr>            <tr>              <td colspan=\"2\" style=\"text-align: center;\">                <p>Bol si úspešne zaregistrovaný. Pre dokončenie registrácie klikni na tento <a href=\'skuska\'>odkaz</a>.</p>              </td>            </tr>            <tr class=\"spacer\" style=\"padding: 20px; display: block\"><td colspan=\"2\"></td></tr>          </tbody>          <tr style=\"background-color: crimson; padding: 10px;\">            <td colspan=\"2\">              <table id=\"footer_tbl\" style=\"width: 100%; font-size: 14px; font-weight: 200; border: 0; text-align: left; height: 20px;\">                <tr>                  <td>                    <p><a target=\"_blank\" href=\"http://localhost:3000/\" style=\"color: black\">Swapify</a> team </p>                  </td>                </tr>              </table>            </td>          </tr>        </table>      </td>    </tr>  </table></body></html>", html);
        }
    }
}
