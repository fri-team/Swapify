using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;

namespace FRITeam.Swapify.Backend
{
    public class AuthenticatorLdap
    {
        private readonly OptionsLdap _options;

        public AuthenticatorLdap(OptionsLdap options)
        {
            _options = options;
        }

        public UserInformations Authenticate(string username, string password, ILogger logger)
        {
            UserInformations userInformations = null;
            logger.LogDebug("LDAP: Trying connection to LDAP");
            var connection = new LdapConnection { SecureSocketLayer = _options.SecureSocketLayer };
            string[] attributes = new[] { "samaccountname", "displayname", "employeeNumber", "mail" };
            connection.Connect(_options.HostName, _options.Port);
            connection.Bind(username, password);
            if (connection.Bound)
            {
                logger.LogDebug("LDAP: Connected to LDAP with username " + username);
                ILdapSearchResults results = connection.Search(_options.BaseDN, LdapConnection.ScopeSub,
                    $"samaccountname={username.Split("@")[0]}", attributes, false);

                if (results.HasMore())
                {
                    var attributeSet = results.Next().GetAttributeSet();
                    logger.LogDebug("LDAP: LDAP has data for user " + username);
                    userInformations = new UserInformations
                    {
                        Name = attributeSet.GetAttribute("displayname")?.StringValue,
                        Login = attributeSet.GetAttribute("samaccountname")?.StringValue,
                        PersonalNumber = attributeSet.GetAttribute("employeeNumber")?.StringValue,
                        Email = attributeSet.GetAttribute("mail")?.StringValue,
                    };
                }
            }
            return userInformations;
        }
    }
    public class UserInformations
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string PersonalNumber { get; set; }
        public string Email { get; set; }
    }

    public class OptionsLdap
    {
        public bool SecureSocketLayer { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string BaseDN { get; set; }
    }
}
