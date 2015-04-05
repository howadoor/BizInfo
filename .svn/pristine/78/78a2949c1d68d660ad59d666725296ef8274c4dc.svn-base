using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Perenis.Core.Pattern;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Implementation of <see cref="IDeserializer"/>
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// using (var stream = new FileStream(@"c:\test.xml", FileMode.Open, FileAccess.Read))
    /// {
    ///     using (var xmlReader = XmlReader.Create(stream))
    ///     {
    ///         var deserializer = new Deserializer(xmlReader);
    ///         hall = (IHall)deserializer.ReadObjects().Single();
    ///     }
    /// } 
    /// ]]>
    /// </code>
    /// </example>
    public class Deserializer : IDeserializer, IDisposable
    {
        private readonly IDictionary<string, object> objects;
        private IInstantiatorProvider instantiatorProvider;
        private IItemsAdderProvider itemsAdderProvider;
        private IPropertySetterProvider propertySetterProvider;

        public Deserializer(XmlReader reader) : this(XmlDictionaryReader.CreateDictionaryReader(reader))
        {
        }

        public Deserializer(XmlDictionaryReader reader)
        {
            objects = new Dictionary<string, object>();
            Infra = new Dictionary<string, string>();
            Reader = reader;
            reader.MoveToStartElement();
            if (reader.LocalName != Constants.RootName) throw new UnexpectedRootNameException(this);
            if (reader.NamespaceURI != Constants.InfraNs) throw new UnexpectedRootNamespaceException(this);
            ReadInfrastructureInformations();
        }

        public IInstantiatorProvider InstantiatorProvider
        {
            get { return instantiatorProvider ?? Singleton<BasicInstantiatorProvider>.Instance; }
            set { instantiatorProvider = value; }
        }

        public IItemsAdderProvider ItemsAdderProvider
        {
            get { return itemsAdderProvider ?? Singleton<BasicItemsAdderProvider>.Instance; }
            set { itemsAdderProvider = value; }
        }

        public IPropertySetterProvider PropertySetterProvider
        {
            get { return propertySetterProvider ?? Singleton<BasicPropertySetterProvider>.Instance; }
            set { propertySetterProvider = value; }
        }

        /// <summary>
        /// Infrastructure informations
        /// </summary>
        public IDictionary<string, string> Infra { get; private set; }

        #region IDeserializer Members

        public XmlDictionaryReader Reader { get; protected set; }

        public IEnumerable<object> ReadObjects()
        {
            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType != XmlNodeType.Element) continue;
                // read local tag
                if (string.IsNullOrEmpty(Reader.NamespaceURI))
                {
                    yield return ReadObject(Reader.Name);
                    continue;
                }
                // read infrastructure tag 
                if (Reader.NamespaceURI == Constants.InfraNs)
                {
                    object @object;
                    if (ReadInfrastructureObject(out @object)) yield return @object;
                    continue;
                }
                // skip all tags from foreign namespaces
                Reader.Skip();
                continue;
            }
        }

        public object GetObject(string objectId)
        {
            if (!objects.ContainsKey(objectId)) throw new InvalidIdException(this, objectId);
            return objects[objectId];
        }

        public void AddObject(string id, object @object)
        {
            if (objects.ContainsKey(id)) throw new ObjectIdNotUniqueException(this, id, @object);
            objects[id] = @object;
        }

        #endregion

        private void ReadInfrastructureInformations()
        {
            for (;;)
            {
                Reader.ReadStartElement();
                Reader.MoveToStartElement();
                if (string.IsNullOrEmpty(Reader.NamespaceURI)) throw new UnexpectedTagException(this, Constants.InfraName);
                if (Reader.NamespaceURI == Constants.InfraNs)
                {
                    if (Reader.LocalName != Constants.InfraName) throw new UnexpectedNamespaceException(this, Constants.InfraNs);
                    break;
                }
                Reader.Skip();
            }
            Reader.MoveToContent();
            this.ForInfrastructureElements(() =>
                                               {
                                                   var infraKey = Reader.LocalName;
                                                   var infraValue = Reader.ReadElementString();
                                                   Infra[infraKey] = infraValue;
                                               });
        }

        /// <summary>
        /// It is assured that element is from infra namespace
        /// </summary>
        /// <param name="object"></param>
        /// <returns><c>true</c> if some data object was created</returns>
        private bool ReadInfrastructureObject(out object @object)
        {
            switch (Reader.LocalName)
            {
                case Constants.NullName:
                    @object = null;
                    return true;

                case Constants.SchemeName:
                    ReadScheme();
                    @object = null;
                    return false;

                case Constants.RefName:
                    @object = GetObject(Reader.GetAttribute(Constants.TargetName, Constants.InfraNs));
                    return true;

                default:
                    throw new UnexpectedInfrastructureTagException(this);
            }
        }

        public void ReadScheme()
        {
            var id = Reader.GetAttribute(Constants.IdName);
            if (string.IsNullOrEmpty(id)) throw new MissingRequiredAttributeException(this, Constants.IdName);
            var strategyName = Reader.GetAttribute(Constants.StrategyName, Constants.InfraNs);
            if (string.IsNullOrEmpty(strategyName)) throw new MissingRequiredAttributeException(this, Constants.StrategyName);
            var strategy = GetStrategy(strategyName);
            var scheme = strategy.ReadScheme(this);
            AddObject(id, scheme);
        }

        private ISerializationStrategy GetStrategy(string strategyName)
        {
            switch (strategyName)
            {
                case Constants.StringSerializationStrategyName:
                    return Singleton<StringSerializationStrategy>.Instance;
                case Constants.TypeSerializationStrategyName:
                    return Singleton<TypeSerializationStrategy>.Instance;
                case Constants.BytesSerializationStrategyName:
                    return Singleton<BytesSerializationStrategy>.Instance;
                case Constants.ConvertToStringSerializationStrategyName:
                    return Singleton<ConvertToStringSerializationStrategy>.Instance;
                case Constants.ByPropertiesSerializationStrategyName:
                    return Singleton<ByPropertiesSerializationStrategy>.Instance;
                default:
                    throw new UknownStrategyException(this, strategyName);
            }
        }

        public object ReadObject(string schemeId)
        {
            // solve reference first
            var refId = Reader.GetAttribute(Constants.RefName, Constants.InfraNs);
            if (!string.IsNullOrEmpty(refId)) return GetObject(refId);
            // scheme can be forced by "scheme" attribute
            var forcedSchemeId = Reader.GetAttribute(Constants.SchemeName, Constants.InfraNs);
            return this.GetScheme(string.IsNullOrEmpty(forcedSchemeId) ? schemeId : forcedSchemeId).ReadObject(this);
        }

        public object ReadObject(IReadingScheme scheme)
        {
            // solve reference first
            var refId = Reader.GetAttribute(Constants.RefName, Constants.InfraNs);
            if (!string.IsNullOrEmpty(refId)) return GetObject(refId);
            // scheme can be forced by "scheme" attribute
            var forcedSchemeId = Reader.GetAttribute(Constants.SchemeName, Constants.InfraNs);
            if (forcedSchemeId == "ThreadSafeListOfILogicalPartAndObservableCollectionOfILogicalPart")
            {
                int c = 20;
            }
            return (string.IsNullOrEmpty(forcedSchemeId) ? scheme : this.GetScheme(forcedSchemeId)).ReadObject(this);
        }

        public void Dispose()
        {
            if (Reader == null) return;
            Reader.Close();
            Reader = null;
        }
    }
}