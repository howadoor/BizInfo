using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Provides an action which sets property of target object to the new value. Used by <see cref="Deserializer"/>.
    /// </summary>
    public interface IPropertySetterProvider
    {
        /// <summary>
        /// Provides an action which sets property of target object to the new value
        /// </summary>
        /// <param name="type">Type of target object</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>
        /// Action which sets property of target object to the new value. First parameter of
        /// action is target object, second parameter is new value of property.
        /// </returns>
        Action<object, object> GetPropertySetter(Type type, string propertyName);
    }
}