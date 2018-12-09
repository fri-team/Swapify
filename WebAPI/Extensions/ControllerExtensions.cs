using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static string IdentityErrorBuilder(string baseErrorMessage, IEnumerable<IdentityError> identityErrors)
        {
            StringBuilder identityErrorBuilder = identityErrors.Aggregate(
                           new StringBuilder(baseErrorMessage),
                           (sb, x) => sb.Append($"{x.Description} "));
            return identityErrorBuilder.ToString();
        }

        internal static Dictionary<string, string[]> IdentityErrorsToDictionary(IEnumerable<IdentityError> errors)
        {
            return errors.ToDictionary(x => x.Code, x => new string[] { x.Description });
        }
    }
}
