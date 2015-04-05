using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Scheme of <c>null</c>
    /// </summary>
    internal class NullWritingScheme : IWritingScheme
    {
        #region IWritingScheme Members

        public ISerializationStrategy Strategy
        {
            get { throw new NotImplementedException(); }
        }

        public Type Type
        {
            get { return null; }
        }

        public string XmlName
        {
            get { return Constants.NullName; }
        }

        public void Write(object @object, ISerializer serializer)
        {
        }

        public void WriteContent(object @object, ISerializer serializer)
        {
        }

        #endregion
    }
}