using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Basic implementation of <see cref="IXmlNameProvider"/>. Creates human-readable and intuitive XML names for
    /// serialization purposes.
    /// </summary>
    internal class BasicXmlNameProvider : IXmlNameProvider
    {
        private readonly Dictionary<object, string> names = new Dictionary<object, string>();

        #region IXmlNameProvider Members

        /// <summary>
        /// Returns XML-conformed name of the <see cref="type"/>. Returned name is unique within
        /// this instance of <see cref="IXmlNameProvider"/> and must be always the same for this type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetXmlName(Type type)
        {
            string name;
            if (!names.TryGetValue(type, out name))
            {
                names[type] = name = CreateXmlName(type);
            }
            return name;
        }

        /// <summary>
        /// Clears all cached XML names in this instance of <see cref="IXmlNameProvider"/>.
        /// </summary>
        public void Clear()
        {
            names.Clear();
        }

        #endregion

        /// <summary>
        /// Creates name of the <see cref="type"/>. It is enssured that it is not used yet for any object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string CreateXmlName(Type type)
        {
            if (type.IsArray)
            {
                return EnsureNotUsedYet(GetXmlName(type.GetElementType()) + "Array");
            }
            if (type.IsGenericType)
            {
                if (type.FindGenericTypeOfDefinition(typeof (KeyValuePair<,>)).FirstOrDefault() != null)
                {
                    var builder = new StringBuilder();
                    bool isFirst = true;
                    foreach (var gdType in type.GetGenericArguments())
                    {
                        if (!isFirst) builder.Append("To");
                        else isFirst = false;
                        builder.Append(GetXmlName(gdType));
                    }
                    return EnsureNotUsedYet(builder.ToString());
                }
                else
                {
                    var uglyIndex = type.Name.IndexOf('`');
                    var builder = new StringBuilder(EnsureNotUsedYet(uglyIndex >= 0 ? type.Name.Substring(0, uglyIndex) : type.Name));
                    builder.Append("Of");
                    bool isFirst = true;
                    foreach (var gdType in type.GetGenericArguments())
                    {
                        if (!isFirst) builder.Append("And");
                        else isFirst = false;
                        builder.Append(GetXmlName(gdType));
                    }
                    return EnsureNotUsedYet(builder.ToString());
                }
            }
            return EnsureNotUsedYet(type.Name);
        }

        /// <summary>
        /// Ensures that the name was not used yet for any other object. If used, adds a number as a suffix.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string EnsureNotUsedYet(string name)
        {
            var resultName = name;
            for (int suffixIndex = 1; names.Values.Contains(resultName); resultName = string.Format("{0}{1}", name, suffixIndex), suffixIndex++) ;
            return resultName;
        }
    }
}