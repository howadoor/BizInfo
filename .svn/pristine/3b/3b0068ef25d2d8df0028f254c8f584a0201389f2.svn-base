using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Provides name-resolution service for <see cref="Type"/> instances.
    /// </summary>
    /// <remarks>
    /// The name constructed for a <see cref="Type"/> instance is the type's full name (i.e. the
    /// value of the <see cref="Type.FullName"/> property).
    /// </remarks>
    public class TypeStructuredNamingResolver : Singleton<TypeStructuredNamingResolver>, IStructuredNamingResolver<object>
    {
        #region ------ Implementation of the IStructuredNamingResolver interface ------------------

        public string GetStructuredName(object nameObject)
        {
            if (nameObject == null) throw new ArgumentNullException("nameObject");
            if (!(nameObject is Type)) return null;
            return ((Type) nameObject).FullName;
        }

        #endregion
    }
}