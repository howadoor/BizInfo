using System;
using System.Collections.Generic;

namespace Perenis.Core.Serialization
{
    internal class StandardSchemeProvider : ISchemeProvider
    {
        private readonly Metascheme metascheme = new Metascheme();
        private readonly Dictionary<Type, IWritingScheme> schemes = new Dictionary<Type, IWritingScheme>();
        private IWritingScheme _nullWritingScheme;

        protected IWritingScheme NullWritingScheme
        {
            get { return _nullWritingScheme ?? (_nullWritingScheme = new NullWritingScheme()); }
        }

        #region ISchemeProvider Members

        public IWritingScheme GetScheme(Type type, ISerializer serializer)
        {
            if (type == null) return NullWritingScheme;
            IWritingScheme writingScheme;
            if (!schemes.TryGetValue(type, out writingScheme))
            {
                schemes[type] = writingScheme = CreateScheme(type, serializer);
                serializer.Write(writingScheme);
            }
            return writingScheme;
        }

        #endregion

        private IWritingScheme CreateScheme(Type type, ISerializer serializer)
        {
            if (typeof (IWritingScheme).IsAssignableFrom(type)) return metascheme;
            if (type == typeof (Type).GetType()) return new TypeWritingScheme(type, serializer.XmlNameProvider.GetXmlName(type.GetType()), serializer);
            if (type == typeof (byte[])) return new BytesWritingScheme(type, serializer.XmlNameProvider.GetXmlName(type), serializer);
            var scheme = new WritingScheme(type, serializer.XmlNameProvider.GetXmlName(type), serializer);
            return scheme;
        }
    }
}