using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Perenis.Core.General
{
    /// <summary>
    /// Adds some useful methods to existing classes.
    /// </summary>
    public static class Extensions
    {
        #region GetCustomAttributes

        /// <summary>
        /// Gets the attributes of the specified type, and return them typed.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to find and to return.</typeparam>
        /// <param name="memberInfo">The memberInfo where the attributes will be searched.</param>
        /// <returns>A typed array with the attributes found.</returns>
        public static T[] GetCustomAttributes<T>(this MemberInfo memberInfo)
            where
                T : Attribute
        {
            return (T[]) memberInfo.GetCustomAttributes(typeof (T), false);
        }

        /// <summary>
        /// Gets the attributes of the specified type, and return them typed.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to find and to return.</typeparam>
        /// <param name="type">The type that can contains the attributes that will be searched.</param>
        /// <param name="inherit">If true search the attribute in base classes, but only if the attribute supports inheritance.</param>
        /// <returns>A typed array with the attributes found.</returns>
        public static T[] GetCustomAttributes<T>(this Type type, bool inherit)
            where
                T : Attribute
        {
            return (T[]) type.GetCustomAttributes(typeof (T), inherit);
        }

        #endregion

        #region GetCustomAttribute

        /// <summary>
        /// Gets an attribute of the specified type, or null.
        /// This is useful when the attribute has AllowMultiple=false, but
        /// don't use it if the class can have more than one attribute of such
        /// type, as this method throws an exception when this happens.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to find.</typeparam>
        /// <param name="memberInfo">The member info to search the attribute.</param>
        /// <returns>The found attribute or null.</returns>
        public static T GetCustomAttribute<T>(this MemberInfo memberInfo)
            where
                T : Attribute
        {
            T[] attributes = memberInfo.GetCustomAttributes<T>();

            switch (attributes.Length)
            {
                case 0:
                    return null;

                case 1:
                    return attributes[0];
            }

            throw new InvalidOperationException("There is more than one attribute of type " + typeof (T).FullName + ".");
        }

        /// <summary>
        /// Gets an attribute of the specified type, or null.
        /// This is useful when the attribute has AllowMultiple=false, but
        /// don't use it if the class can have more than one attribute of such
        /// type, as this method throws an exception when this happens.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to find.</typeparam>
        /// <param name="type">The type to search the attribute.</param>
        /// <param name="inherit">true to search in base classes for attributes that support inheritance.</param>
        /// <returns>The found attribute or null.</returns>
        public static T GetCustomAttribute<T>(this Type type, bool inherit)
            where
                T : Attribute
        {
            T[] attributes = type.GetCustomAttributes<T>(inherit);

            switch (attributes.Length)
            {
                case 0:
                    return null;

                case 1:
                    return attributes[0];
            }

            throw new InvalidOperationException("There is more than one attribute of type " + typeof (T).FullName + ".");
        }

        #endregion

        #region ContainsCustomAttribute

        /// <summary>
        /// Verifies if a member contains an specific custom attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check for existance.</typeparam>
        /// <param name="member">The member in which to find search for attribute.</param>
        /// <returns>true if the member constains the attribute, false otherwise.</returns>
        public static bool ContainsCustomAttribute<T>(this MemberInfo member)
            where
                T : Attribute
        {
            return GetCustomAttributes<T>(member).Length > 0;
        }

        /// <summary>
        /// Verifies if a type contains an specific custom attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check for existance.</typeparam>
        /// <param name="type">The member in which to find search for attribute.</param>
        /// <param name="inherit">true to search in base classes for attributes that support inheritance.</param>
        /// <returns>true if the member constains the attribute, false otherwise.</returns>
        public static bool ContainsCustomAttribute<T>(this Type type, bool inherit)
            where
                T : Attribute
        {
            return GetCustomAttributes<T>(type, inherit).Length > 0;
        }

        #endregion

        #region GetDisplayName

        /// <summary>
        /// Gets the display name of an enumerated value.
        /// If no EnumDisplayName attribute is set, uses the default enum name.
        /// </summary>
        /// <param name="enumValue">The enum value to get the display name.</param>
        /// <returns>The display name.</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException("enumValue");

            string name = enumValue.ToString();
            FieldInfo fieldInfo = enumValue.GetType().GetField(name);
            var attribute = fieldInfo.GetCustomAttribute<DisplayNameAttribute>();

            if (attribute != null)
                return attribute.DisplayName;

            return name;
        }

        /// <summary>
        /// Gets the DisplayName of a member, or it's real name if it does
        /// not have a DisplayName.
        /// </summary>
        /// <param name="member">The member to get the display name for.</param>
        /// <returns>A name.</returns>
        public static string GetDisplayName(this MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<DisplayNameAttribute>();
            if (attribute == null)
                return member.Name;

            return attribute.DisplayName;
        }

        #endregion

        #region GetDirectSubClasses

        private static readonly Dictionary<KeyValuePair<Type, Assembly>, ReadOnlyCollection<Type>> fSubClasses = new Dictionary<KeyValuePair<Type, Assembly>, ReadOnlyCollection<Type>>();

        /// <summary>
        /// Gets the sub-classes of the specific type, in the specific assembly.
        /// </summary>
        public static ReadOnlyCollection<Type> GetDirectSubClasses(this Type type, Assembly inAssembly)
        {
            ReadOnlyCollection<Type> result;
            var pair = new KeyValuePair<Type, Assembly>(type, inAssembly);
            lock (fSubClasses)
            {
                if (!fSubClasses.TryGetValue(pair, out result))
                {
                    var list = new List<Type>();
                    foreach (Type possibleSubType in inAssembly.GetTypes())
                        if (possibleSubType.BaseType == type)
                            list.Add(possibleSubType);

                    result = new ReadOnlyCollection<Type>(list.ToArray());
                    fSubClasses.Add(pair, result);
                }
            }

            return result;
        }

        #endregion

        #region GetSubClassesRecursive

        private static readonly Dictionary<KeyValuePair<Type, Assembly>, ReadOnlyCollection<Type>> fSubClassesRecursive = new Dictionary<KeyValuePair<Type, Assembly>, ReadOnlyCollection<Type>>();

        /// <summary>
        /// Gets the sub-classes of the specific type, in the specific assembly.
        /// </summary>
        public static ReadOnlyCollection<Type> GetSubClassesRecursive(this Type type, Assembly inAssembly)
        {
            ReadOnlyCollection<Type> result;
            var pair = new KeyValuePair<Type, Assembly>(type, inAssembly);
            lock (fSubClassesRecursive)
            {
                if (!fSubClassesRecursive.TryGetValue(pair, out result))
                {
                    var list = new List<Type>();
                    foreach (Type possibleSubType in inAssembly.GetTypes())
                        if (possibleSubType != type && type.IsAssignableFrom(possibleSubType))
                            list.Add(possibleSubType);

                    result = new ReadOnlyCollection<Type>(list.ToArray());
                    fSubClassesRecursive.Add(pair, result);
                }
            }

            return result;
        }

        #endregion
    }
}