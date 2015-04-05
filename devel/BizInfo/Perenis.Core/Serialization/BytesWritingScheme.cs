using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Scheme for array of bytes. Default implementation stores content of array as Base64 string.
    /// Future implementations can store bytes to companion files and refernce it from main file
    /// </summary>
    internal class BytesWritingScheme : WritingScheme, IWritingScheme
    {
        /// <summary>
        /// Creates instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xmlName"></param>
        /// <param name="serializer"></param>
        public BytesWritingScheme(Type type, string xmlName, ISerializer serializer) : base(type, xmlName, serializer)
        {
            Strategy = Singleton<BytesSerializationStrategy>.Instance;
            ItemsWritingScheme = null;
        }

        #region IWritingScheme Members

        /// <summary>
        /// Writes content of array is Base64 string
        /// </summary>
        /// <param name="object"></param>
        /// <param name="serializer"></param>
        public override void WriteContent(object @object, ISerializer serializer)
        {
            WriteId(serializer, @object);
            var array = (byte[]) @object;
            serializer.Writer.WriteBase64(array, 0, array.Length);
        }

        #endregion
    }
}