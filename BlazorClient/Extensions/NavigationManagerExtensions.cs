using System;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static string RelativeUri(this NavigationManager navigationManager)
        {
            return navigationManager.ToBaseRelativePath(navigationManager.Uri);
        }

        public static bool EqualsRelativeUri(this NavigationManager navigationManager, string relativeUri)
        {            
            return navigationManager.ToBaseRelativePath(navigationManager.Uri).Equals(relativeUri);
        }
    }
}
