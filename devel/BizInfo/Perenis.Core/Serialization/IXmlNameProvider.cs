using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Object which is responsible to create XML-conformed tag and attribute name
    /// </summary>
    public interface IXmlNameProvider
    {
        /// <summary>
        /// Returns XML-conformed name of the <see cref="type"/>. Returned name is unique within
        /// this instance of <see cref="IXmlNameProvider"/> and must be always the same for this type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetXmlName(Type type);

        /// <summary>
        /// Clears all cached XML names in this instance of <see cref="IXmlNameProvider"/>.
        /// </summary>
        void Clear();
    }
}