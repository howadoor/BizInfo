using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Perenis.Core.Collections;
using Perenis.Core.Reflection;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    internal class PropertyReader : IPropertyReader
    {
        #region IPropertyReader Members

        public string XmlName { get; set; }

        public IReadingScheme PropertyScheme { get; set; }

        public Action<object, object> PropertySetter { get; set; }

        public object ReadProperty(IDeserializer deserializer)
        {
            return ((Deserializer) deserializer).ReadObject(PropertyScheme);
        }

        #endregion

        private MethodInfo GetClearMethod(object targetObject)
        {
            return targetObject.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, new Type[]{}, new ParameterModifier[]{});
        }

        public override string ToString()
        {
            return string.Format("Reader of {0} property of {1} type", XmlName, PropertyScheme.Type);
        }
    }
}