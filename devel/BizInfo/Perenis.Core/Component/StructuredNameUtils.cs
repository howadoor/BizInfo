using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Utility class defining tasks related to structured names, <see cref="StructuredNamingResolver{T}"/>.
    /// </summary>
    public static class StructuredNameUtils
    {
        /// <summary>
        /// Retrieves a structured name for the provided program component <paramref name="memberInfo"/>.
        /// </summary>
        /// <param name="memberInfo">Program component</param>
        /// <returns>A structured name if found, null otherwise.</returns>
        public static object GetStructuredName(MemberInfo memberInfo)
        {
            return Singleton<AttributeManagerRegistry>.Instance.Get(memberInfo).GetAttribute<StructuredNameAttribute>();
        }
    }
}