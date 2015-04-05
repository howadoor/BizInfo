using System;
using System.Collections;
using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    internal class BasicPropertySetterProvider : IPropertySetterProvider
    {
        #region IPropertySetterProvider Members

        public Action<object, object> GetPropertySetter(Type type, string propertyName)
        {
            // first try to set property
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                return (target, newValue) => propertyInfo.SetValue(target, newValue, null);
            }
            // try to set field
            var fieldInfo = type.GetField(propertyName);
            if (fieldInfo != null)
            {
                return (target, newValue) => fieldInfo.SetValue(target, newValue);
            }
            // impossible - so try to copy items from new value collection to current property value collection
            var propertyGetter = GetPropertyGetter(type, propertyName);
            if (propertyGetter == null) throw new InvalidOperationException("Cannot create setter");
            return (target, newValue) =>
                       {
                           var currentValue = propertyGetter(target);
                           if (currentValue == newValue) return;
                           if (currentValue is IEnumerable && newValue is IEnumerable)
                           {
                               // clear old collection and add all items
                               var clearMethod = GetClearMethod(currentValue);
                               if (clearMethod != null)
                               {
                                   clearMethod.Invoke(currentValue, new object[] {});
                               }
                               var adder = Singleton<BasicItemsAdderProvider>.Instance.GetItemsAdder(currentValue.GetType()) ;
                               foreach (var item in (IEnumerable) newValue) adder(currentValue, item);
                               return;
                           }
                           throw new NoWayForSettingPropertyException(null, null, propertyName, target, newValue);
                       };
        }

        #endregion

        private MethodInfo GetClearMethod(object targetObject)
        {
            return targetObject.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, new Type[]{}, new ParameterModifier[]{});
        }
        
        private Func<object, object> GetPropertyGetter (Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanRead)
            {
                return source => propertyInfo.GetValue(source, null);
            }
            var fieldInfo = type.GetField(propertyName);
            if (fieldInfo != null)
            {
                return source => fieldInfo.GetValue(source);
            }
            return null;
        }
    }
}