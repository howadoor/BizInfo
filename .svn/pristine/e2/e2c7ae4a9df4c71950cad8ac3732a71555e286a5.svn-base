using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Implementation of <see cref="ISerializer"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// using (var stream = new FileStream(@"c:\test.xml", FileMode.Create, FileAccess.Write))
    /// {
    ///     var xmlWriterSettings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
    ///     using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
    ///     {
    ///         var serializer = new Serializer(xmlWriter);
    ///         serializer.Write(hall);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class Serializer : ISerializer, IDisposable
    {
        public Serializer(XmlWriter writer) : this(XmlDictionaryWriter.CreateDictionaryWriter(writer))
        {
        }

        public Serializer(XmlDictionaryWriter writer) : this(writer, new Dictionary<string, string>(4))
        {
        }

        public Serializer(XmlDictionaryWriter writer, IDictionary<string, string> infra)
        {
            Writer = writer;
            SchemeProvider = new StandardSchemeProvider();
            XmlNameProvider = new BasicXmlNameProvider();
            IdProvider = new IdProvider();
            Infra = infra;
            writer.WriteStartDocument();
            writer.WriteComment("\r\n" + Properties.Resources.SerializerWarning + "\r\n");
            writer.WriteStartElement(Constants.InfraPrefix, Constants.RootName, Constants.InfraNs);
            SetupInfrastructureInformations();
            WriteInfrastructureInformations();
        }

        /// <summary>
        /// Infrastructure informations
        /// </summary>
        public IDictionary<string, string> Infra { get; private set; }

        #region ISerializer Members

        public IXmlNameProvider XmlNameProvider { get; private set; }

        public XmlDictionaryWriter Writer { get; protected set; }
        public ISchemeProvider SchemeProvider { get; protected set; }

        public IIdProvider IdProvider { get; protected set; }

        public void Write(object @object)
        {
            if (@object == null)
            {
                Writer.WriteElementString(Constants.InfraPrefix, Constants.NullName, Constants.InfraNs, null);
                return;
            }
            if (IdProvider.HasId(@object))
            {
                Writer.WriteStartElement(Constants.InfraPrefix, Constants.RefName, Constants.InfraNs);
                Writer.WriteAttributeString(Constants.InfraPrefix, Constants.TargetName, Constants.InfraNs, IdProvider.GetId(@object));
                Writer.WriteEndElement();
                return;
            }
            var scheme = SchemeProvider.GetScheme(@object.GetType(), this);
            scheme.Write(@object, this);
        }

        public void Dispose()
        {
            if (Writer == null) return;
            Writer.Close();
            Writer = null;
        }

        #endregion

        private void SetupInfrastructureInformations()
        {
            if (Infra == null) return;
            if (!Infra.ContainsKey(Constants.CreationDateName)) Infra[Constants.CreationDateName] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat.SortableDateTimePattern);
            if (!Infra.ContainsKey(Constants.AuthorName)) Infra[Constants.AuthorName] = string.Format("{0}@{1}", Environment.UserDomainName, Environment.UserName);
            if (!Infra.ContainsKey(Constants.SerializerName)) Infra[Constants.SerializerName] = GetType().AssemblyQualifiedName;
            if (!Infra.ContainsKey(Constants.GeneratorName) && Assembly.GetEntryAssembly() != null) Infra[Constants.GeneratorName] = Assembly.GetEntryAssembly().FullName;
        }

        private void WriteInfrastructureInformations()
        {
            if (Infra == null) return;
            Writer.WriteStartElement(Constants.InfraPrefix, Constants.InfraName, Constants.InfraNs);
            var sortedKeys = Infra.Keys.ToArray();
            Array.Sort(sortedKeys, StringComparer.InvariantCultureIgnoreCase);
            foreach (var key in sortedKeys)
            {
                Writer.WriteElementString(Constants.InfraPrefix, key, Constants.InfraNs, Infra[key]);
            }
            Writer.WriteEndElement();
        }
    }
}