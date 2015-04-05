using System.Reflection;

namespace BizInfo.App.Services.Tools
{
    public static class AssemblyTools
    {
        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>The assembly copyright.</value>
        public static string GetCopyright(this Assembly assembly)
        {
            // Get all Copyright attributes on this assembly
            var attributes = assembly.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
            // If there aren't any Copyright attributes, return an empty string
            if (attributes.Length == 0)
                return null;
            // If there is a Copyright attribute, return its value
            return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
        }
    }
}