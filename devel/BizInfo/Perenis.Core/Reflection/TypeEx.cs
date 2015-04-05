using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Colects extension methods of <see cref="Type"/>
    /// </summary>
    public static class TypeEx
    {
        /// <summary>
        /// Returns default of <see cref="Type"/>, eqivalent of default(type).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Default(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Returns all fields of the type, either inherited from <see cref="type.BaseType"/> type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetAllFields(this Type type, BindingFlags bindingFlags)
        {
            if (type.BaseType == null) return type.GetFields(bindingFlags);
            return type.BaseType.GetAllFields(bindingFlags).Concat(type.GetFields(bindingFlags)); 
        }
    
        /// <summary>
        /// Returns all types in type hierarchy which is defined by given generic type definition
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindGenericTypeOfDefinition (this Type type, Type genericTypeDefinition)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition) yield return type;
            if (type.BaseType != null) foreach (var found in FindGenericTypeOfDefinition(type.BaseType, genericTypeDefinition)) yield return found;
            foreach (var @interface in type.GetInterfaces())
            {
                foreach (var found in FindGenericTypeOfDefinition(@interface, genericTypeDefinition)) yield return found;
            }
        }

        /// <summary>
        /// Enumerates all methods even in interface types. See http://stackoverflow.com/questions/4216908/get-all-methods-on-an-type-of-interfaces-using-linq
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            return type.GetMethods().Concat(type.GetInterfaces().SelectMany(i => i.GetMethods()));
        }

        /// <summary>
        /// Creates parametersless function which returns new instance of <see cref="type"/> each time called. Using
        /// <see cref="FormatterServices"/> to create new instance and initializes all field with default values of field types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<object> GetInstantiatorUsingFormatterServices(this Type type)
        {
            var members = type.GetFields();
            var initializationData = members.Select(member => member.FieldType.Default()).ToArray();
            return () =>
                       {
                           var @object = FormatterServices.GetUninitializedObject(type);
                           FormatterServices.PopulateObjectMembers(@object, members, initializationData);
                           return @object;
                       };
        }

        /// <summary>
        /// Returns parametersless constructor, either private or public, or <c>null</c> if non exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetParameterlessConstructor(this Type type)
        {
            return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, new ParameterModifier[] { });
        }

        /// <summary>
        /// Provides parametersless function which creates new instance of given <see cref="type"/> each time called
        /// </summary>
        /// <remarks>
        /// Tries to find parameterless constructor, either private or public. Is fails, uses <see cref="TypeEx.GetInstantiatorUsingFormatterServices"/> to create
        /// instantiator.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>Parametersless function which creates new instance of given <see cref="type"/> each time called</returns>
        public static Func<object> GetInstantiator(this Type type)
        {
            var constructor = type.GetParameterlessConstructor();
            if (constructor != null) return () => constructor.Invoke(new object[] { });
            return type.GetInstantiatorUsingFormatterServices();
        }
    }
}