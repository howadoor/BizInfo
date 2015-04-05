using System.Collections.Generic;
using System.Xml;

namespace Perenis.Core.Serialization
{
    public interface IDeserializer
    {
        XmlDictionaryReader Reader { get; }

        IEnumerable<object> ReadObjects();

        object GetObject(string objectId);

        void AddObject(string id, object @object);
    }
}