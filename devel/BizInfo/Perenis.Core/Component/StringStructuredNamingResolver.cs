using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Provides name-resolution service for string values.
    /// </summary>
    /// <remarks>
    /// The name constructed for a string value is the value itself.
    /// </remarks>
    public class StringStructuredNamingResolver : Singleton<StringStructuredNamingResolver>, IStructuredNamingResolver<object>
    {
        #region ------ Implementation of the IStructuredNamingResolver interface ------------------

        public string GetStructuredName(object nameObject)
        {
            if (nameObject == null) throw new ArgumentNullException("nameObject");
            if (!(nameObject is string)) return null;
            return (string) nameObject;
        }

        #endregion
    }
}