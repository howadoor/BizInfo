using System;
using System.Reflection;

namespace Perenis.Core.General
{
    /// <summary>
    /// This is a abstract class that implements the ICloneable interface using the 
    /// MemberwiseClone method, and then also clones all ICloneable objects.
    /// </summary>
    [Serializable]
    public abstract class Cloneable :
        ICloneable
    {
        #region Clone

        /// <summary>
        /// Clones this object and also all it's fields that references ICloneable objects.
        /// </summary>
        /// <returns>A memberwise cloned object, with all that are ICloneable also cloned.</returns>
        protected internal virtual Cloneable Clone()
        {
            object clonedCloneable = MemberwiseClone();

            Type type = GetType();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Type propertyType = property.PropertyType;

                if (!property.CanRead)
                    continue;

                if (!property.CanWrite)
                    continue;

                object value = property.GetValue(this, null);
                var cloneable = value as ICloneable;
                if (cloneable != null)
                {
                    object clonedFieldValue = cloneable.Clone();
                    property.GetSetMethod(true).Invoke(clonedCloneable, new[] {clonedFieldValue});
                }
            }

            return (Cloneable) clonedCloneable;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}

/// <summary>
/// This extension class simple creates the typed Clone method to the Cloneable,
/// object, so you clone the object and don't need to cast it to the right type.
/// </summary>
public static class PfzCloneable_Extensions
{
    /// <summary>
    /// Clones the cloneable object and return the clone with the same type
    /// as the original object is seen.
    /// </summary>
    /// <typeparam name="T">The type of the cloneable object.</typeparam>
    /// <param name="cloneable">The object to clone.</param>
    /// <returns>The right-typed clone.</returns>
    public static T Clone<T>(this T cloneable)
    {
        return cloneable.Clone();
    }
}