using FRITeam.Swapify.SwapifyBase.Exceptions;
using FRITeam.Swapify.SwapifyBase.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace WebAPI.Filters
{
    public class SettingValidationFilter : IStartupFilter
    {
        readonly IEnumerable<IValidatable> _validatableObjects;
        public SettingValidationFilter(IEnumerable<IValidatable> validatableObjects)
        {
            _validatableObjects = validatableObjects;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            foreach (var validatableObject in _validatableObjects)
            {
                Console.WriteLine("validatableObject.ConfigFileName: " + validatableObject.ConfigFileName);
                Console.WriteLine("validatableObject.ConfigFileName: " + validatableObject);
                validatableObject.Validate();
                if (validatableObject.Errors.Length != 0)
                    throw new SettingException(validatableObject.ConfigFileName, validatableObject.Errors.ToString());                
            }

            //don't alter the configuration
            return next;
        }
    }
}
