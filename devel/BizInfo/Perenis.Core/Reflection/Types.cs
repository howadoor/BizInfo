using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Perenis.Core.Collections;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Utility pro práci s typy.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Vyhledá všechny typy v zadaném namespace (a vnořených), bez ohledu na assembly původu.
        /// </summary>
        /// <remarks>
        /// Ignorují se assembly uvedené v seznamu <see cref="Assemblies.GlobalIgnoredAssemblies"/>.
        /// </remarks>
        /// <param name="nmspace">Namespace, ve kterém se mají typy hledat.</param>
        /// <param name="ignoreAssemblies">Seznam dodatečných ignorovaných namespaců</param>
        /// <returns>Seznam typů v daném namespace.</returns>
        public static Type[] GetTypesWithinNamespace(string nmspace, Set<string> ignoreAssemblies)
        {
            if (nmspace == null) throw new ArgumentNullException("nmspace");

            // rozšířit jméno namespace o tečku pro jednoznačnost prefixu jména
            if (nmspace.Length > 0) nmspace = nmspace + ".";

            // prohledat všechny assembly a najít typy v zadaném namespace
            try
            {
                var nsTypes = new List<Type>();
                foreach (Assembly a in Assemblies.EnumerateAllAssemblies(ignoreAssemblies))
                {
                    // vyfiltrovat typy podle namespace
                    Type[] types = a.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.FullName.StartsWith(nmspace)) nsTypes.Add(type);
                    }
                }
                return nsTypes.ToArray();
            }
            catch (ReflectionTypeLoadException e)
            {
                var sb = new StringBuilder("Unable to load one or more of the requested types:\n");
                for (int i = 0; i < e.LoaderExceptions.Length; ++i)
                {
                    sb.Append("Exception when loading ");
                    sb.Append(e.Types[i].FullName);
                    sb.Append(": ");
                    sb.Append(e.LoaderExceptions[i].Message);
                    sb.Append('\n');
                }
                throw new TypeLoadException(sb.ToString(), e);
            }
        }

        /// <summary>
        /// Vyhledá všechny typy v zadaném namespace (a vnořených), bez ohledu na assembly původu.
        /// </summary>
        /// <remarks>
        /// Ignorují se assembly uvedené v seznamu <see cref="Assemblies.GlobalIgnoredAssemblies"/>.
        /// </remarks>
        /// <param name="nmspace">Namespace, ve kterém se mají typy hledat.</param>
        /// <returns>Seznam typů v daném namespace.</returns>
        public static Type[] GetTypesWithinNamespace(string nmspace)
        {
            return GetTypesWithinNamespace(nmspace, (Set<string>) null);
        }

        /// <summary>
        /// Vyhledá všechny typy v zadaném namespace (a vnořených) uvnitř dané assembly.
        /// </summary>
        /// <param name="nmspace">Namespace, ve kterém se mají typy hledat.</param>
        /// <param name="assembly">Assembly, ve kterém se mají typy hledat.</param>
        /// <returns>Seznam typů v daném assembly, které patří do daného namespace.</returns>
        public static Type[] GetTypesWithinNamespace(string nmspace, Assembly assembly)
        {
            if (nmspace == null) throw new ArgumentNullException("nmspace");
            if (assembly == null) throw new ArgumentNullException("assembly");

            // rozšířit jméno namespace o tečku pro jednoznačnost prefixu jména
            if (nmspace.Length > 0) nmspace = nmspace + ".";

            return Array.FindAll(assembly.GetTypes(), t => t.FullName.StartsWith(nmspace));
        }

        /// <summary>
        /// Retrieves all ancestors of the given <paramref name="rootType"/> located in
        /// the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="rootType">Root type.</param>
        /// <param name="assembly">The assembly where to look for types.</param>
        /// <returns>Descendants of the <paramref name="rootType"/>.</returns>
        public static IEnumerable<Type> GetDescendants(Type rootType, Assembly assembly)
        {
            if (rootType == null) throw new ArgumentNullException("rootType");
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(rootType)) yield return type;
            }
        }

        /// <summary>
        /// Retrieves all ancestors of the given <paramref name="rootType"/> across all loaded assemblies.
        /// </summary>
        /// <param name="rootType">Root type.</param>
        /// <returns>Descendants of the <paramref name="rootType"/>.</returns>
        public static IEnumerable<Type> GetDescendants(Type rootType)
        {
            if (rootType == null) throw new ArgumentNullException("rootType");

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(rootType)) yield return type;
                }
            }
        }

        /// <summary>
        /// Retrieves a list of all ancestors of the given <paramref name="type"/> including itself.
        /// </summary>
        /// <param name="type">Root type.</param>
        /// <returns>Ancestors of the <paramref name="type"/> including itself.</returns>
        public static List<Type> GetAncestorsAndSelf(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var result = new List<Type>();
            result.Add(type);
            while (!typeof (object).Equals(type))
            {
                type = type.BaseType;
                result.Add(type);
            }
            return result;
        }

        /// <summary>
        /// Gets the types of the objects in the specified array.
        /// </summary>
        /// <param name="args">An array of objects whose types to determine.</param>
        /// <returns>An array of <see cref="Type"/> objects representing the types of the corresponding
        /// elements in args. Returns an empty array if <paramref name="args"/> is a null reference.</returns>
        /// <remarks>
        /// <para>
        /// This method extends the functionality of the <see cref="Type.GetTypeArray"/> method. For
        /// each item that is a null reference, this method returns a <c>null</c>.
        /// </para>
        /// </remarks>
        public static Type[] GetTypeArray(params object[] args)
        {
            if (args == null) return new Type[] {};
            var typeArray = new Type[args.Length];
            for (int i = 0; i < typeArray.Length; i++)
            {
                typeArray[i] = args[i] != null ? args[i].GetType() : null;
            }
            return typeArray;
        }

        /// <summary>
        /// Zjistí, zda T1 je potomek nebo shodný jako T2.
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T2"></param>
        /// <returns></returns>
        public static bool IsDescendantOrEqual(Type T1, Type T2)
        {
            if (T1 == null) throw new ArgumentNullException("T1");
            if (T2 == null) throw new ArgumentNullException("T2");

            if (T2.IsGenericTypeDefinition)
            {
                while (T1 != null)
                {
                    if (T1.IsGenericType && T2.Equals(T1.GetGenericTypeDefinition())) return true;
                    T1 = T1.BaseType;
                }
                return false;
            }
            else
            {
                return T1.IsSubclassOf(T2) || T1.Equals(T2);
            }
        }

        /// <summary>
        /// Zjistí, zda má typ uvedený interface.
        /// </summary>
        /// <param name="type">Testovaný typ.</param>
        /// <param name="iface">Hledaný interface.</param>
        /// <returns>T-má tento interface</returns>
        public static bool HasInterface(Type type, Type iface)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (iface == null) throw new ArgumentNullException("iface");

            foreach (Type exposed in type.GetInterfaces()) if (exposed.Equals(iface)) return true;
            return false;
        }

        /// <summary>
        /// Checks if the given type is a nullable value type.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns><c>true</c> when the given <paramref name="type"/> is a <see cref="Nullable{T}"/>
        /// descendant (e.g. <c>int?</c> or <c>Nullable&lt;bool&gt;</c>); otherwise, <c>false</c>.
        /// This method also returns <c>false</c> when the generic type definition of <see cref="Nullable{T}"/>
        /// is supplied.</returns>
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
        }

        /// <summary>
        /// Searches the given <paramref name="type"/> and all it's ancestors for methods matching the
        /// given <see cref="BindingFlags"/> and the optional <paramref name="filter"/>.
        /// </summary>
        /// <param name="type">The type to be searched.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> 
        /// that specify how the search is conducted.</param>
        /// <param name="filter">An optional member filter.</param>
        /// <param name="filterCriteria">An optional criteria passed to the <paramref name="filter"/>.</param>
        /// <returns>A filtered enumeration of the <paramref name="type"/>'s methods matching the criteria.</returns>
        public static IList<MethodInfo> FindMethods(Type type, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            var result = new List<MethodInfo>();
            foreach (Type t in GetAncestorsAndSelf(type))
            {
                foreach (MethodInfo method in t.GetMethods(bindingAttr | BindingFlags.DeclaredOnly))
                {
                    if (filter == null || filter(method, filterCriteria))
                    {
                        Debug.Assert(!result.Contains(method));
                        result.Add(method);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a list of type members with a specified custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The custom attribute type.</typeparam>
        /// <param name="type">The type to be searched.</param>
        /// <param name="bindingFlags">Binding flags to be used when searching the <paramref name="type"/>.</param>
        /// <param name="inherit">The flag is used in the call to <see cref="MemberInfo.GetCustomAttributes(bool)"/>.</param>
        /// <returns>Returns a list of <see cref="Tuple{MemberInfo, TAttribute}"/> where <see cref="Tuple{T1,T2}.Item1"/> indicates the found <see cref="MemberInfo"/> and
        /// <see cref="Tuple{T1,T2}.Item2"/> indicates the corresponding <see cref="Attribute"/> instance.</returns>
        public static IList<Tuple<MemberInfo, TAttribute>> GetMembersWithAttribute<TAttribute>(Type type, BindingFlags bindingFlags, bool inherit)
            where TAttribute : Attribute
        {
            var result = new List<Tuple<MemberInfo, TAttribute>>();

            MemberInfo[] members = type.FindMembers(MemberTypes.All, bindingFlags, null, null);
            foreach (MemberInfo member in members)
            {
                object[] attrs = member.GetCustomAttributes(typeof (TAttribute), inherit);
                if (attrs.Length == 0) continue;

                foreach (object attr in attrs)
                {
                    result.Add(new Tuple<MemberInfo, TAttribute>(member, (TAttribute) attr));
                }
            }

            return result;
        }

        /// <summary>
        /// Extends the standard behavior of <see cref="Type.GetMembers(BindingFlags)"/> by recursively getting 
        /// members on all the parent interfaces if the supplied type is an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static ICollection<MemberInfo> GetMembers(Type type, BindingFlags bindingFlags)
        {
            var result = new Set<MemberInfo>();
            GetMembers(type, bindingFlags, (t, f) => t.GetMembers(f), result);
            return result;
        }

        /// <summary>
        /// Extends the standard behavior of <see cref="Type.GetProperties(BindingFlags)"/> by recursively getting 
        /// properties on all the parent interfaces if the supplied type is an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static ICollection<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags)
        {
            var result = new Set<PropertyInfo>();
            GetMembers(type, bindingFlags, (t, f) => t.GetProperties(f), result);
            return result;
        }

        /// <summary>
        /// Extends the standard behavior of <see cref="Type.GetMethods(BindingFlags)"/> by recursively getting 
        /// methods on all the parent interfaces if the supplied type is an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static ICollection<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags)
        {
            var result = new Set<MethodInfo>();
            GetMembers(type, bindingFlags, (t, f) => t.GetMethods(f), result);
            return result;
        }

        // TODO unit tests
        private static void GetMembers<T>(Type type, BindingFlags bindingFlags, Func<Type, BindingFlags, IEnumerable<T>> membersFetcher, Set<T> result)
        {
            result.AddRange(membersFetcher(type, bindingFlags));
            if (!type.IsInterface) return;

            foreach (Type @interface in type.GetInterfaces())
            {
                GetMembers(@interface, bindingFlags, membersFetcher, result);
            }

            return;
        }

        #region ------ Unit tests -----------------------------------------------------------------

#if NUNIT

    /// <summary>
    /// Unit tests for the <see cref="Types"/> class.
    /// </summary>
        [TestFixture]
        public class Types_Tests
        {
            class T0 {}
            class TG1<T,U> : T0 {}
            class TG2<T> : TG1<T, int> {}
            class T1 : TG1<int, int> {}
            class T2 : T1 {}
            class T3 : TG2<long> {}
            class T4 : T3 {}
            class T5 : TG2<int> {}

            // TODO Make a unit test for the HasInterface method

            /// <summary>
            /// Tests of the <see cref="IsDescendantOrEqual"/> method.
            /// </summary>
            [Test]
            public void TestIsDescendantOrEqual()
            {
                // equality tests
                Assert.IsTrue(IsDescendantOrEqual(typeof(T0), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(TG1<,>), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(TG2<>), typeof(TG2<>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T1), typeof(T1)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T2), typeof(T2)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T3), typeof(T3)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T4), typeof(T4)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T5), typeof(T5)));

                // descendant tests
                Assert.IsTrue(IsDescendantOrEqual(typeof(TG1<,>), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(TG2<>), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(TG2<>), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T1), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T1), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T2), typeof(T1)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T2), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T2), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T3), typeof(TG2<>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T3), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T3), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T4), typeof(T3)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T4), typeof(TG2<>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T4), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T4), typeof(T0)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T5), typeof(TG2<>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T5), typeof(TG1<,>)));
                Assert.IsTrue(IsDescendantOrEqual(typeof(T5), typeof(T0)));

                // unrelated types tests
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(TG1<,>)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(TG2<>)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T0), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(TG2<>)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG1<,>), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG2<>), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG2<>), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG2<>), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG2<>), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(TG2<>), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T1), typeof(TG2<>)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T1), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T1), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T1), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T1), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T2), typeof(TG2<>)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T2), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T2), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T2), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T3), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T3), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T3), typeof(T4)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T3), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T4), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T4), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T4), typeof(T5)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T5), typeof(T1)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T5), typeof(T2)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T5), typeof(T3)));
                Assert.IsFalse(IsDescendantOrEqual(typeof(T5), typeof(T4)));
            }

            /// <summary>
            /// Tests the <see cref="GetTypeArray"/> method.
            /// </summary>
            [Test]
            public void TestGetTypeArray()
            {
                Assert.AreEqual(new Type[] {typeof (int), typeof (string)}, GetTypeArray(0, "string"));
                Assert.AreEqual(new Type[] {}, GetTypeArray());
                Assert.AreEqual(new Type[] {typeof (T1)}, GetTypeArray(new T1()));
            }

            /// <summary>
            /// Tests of the <see cref="Types.GetDescendants"/> method.
            /// </summary>
            [Test]
            public void TestGetDescendants()
            {
                List<Type> result = new List<Type>(GetDescendants(typeof(T0), typeof(T0).Assembly));
                CollectionAssert.AreEquivalent(
                    new Type[]
                        {
                            typeof(TG1<,>),
                            typeof(TG2<>),
                            typeof(T1),
                            typeof(T2),
                            typeof(T3),
                            typeof(T4),
                            typeof(T5),
                        },
                    result);
            }

            /// <summary>
            /// Tests of the <see cref="Types.GetAncestorsAndSelf"/> method.
            /// </summary>
            [Test]
            public void TestGetAncestorsAndSelf()
            {
                List<Type> result = GetAncestorsAndSelf(typeof (T4));
                CollectionAssert.AreEquivalent(
                    new Type[]
                        {
                            typeof(T4),
                            typeof(T3),
                            typeof(TG2<long>),
                            typeof(TG1<long, int>),
                            typeof(T0),
                            typeof(object),
                        },
                    result);
            }

            /// <summary>
            /// Tests of the <see name="IsNullable"/> method.
            /// </summary>
            [Test]
            public void TestIsNullable()
            {
                Assert.IsTrue(IsNullable(typeof(Nullable<int>)));
                Assert.IsFalse(IsNullable(typeof(Nullable)));
                Assert.IsFalse(IsNullable(typeof(Nullable<>)));
                Assert.IsTrue(IsNullable(typeof(int?)));
                Assert.IsFalse(IsNullable(typeof(int)));
                Assert.IsFalse(IsNullable(typeof(TG2<int>)));
                Assert.IsFalse(IsNullable(typeof(TG2<>)));
                Assert.IsFalse(IsNullable(typeof(string)));
                Assert.IsFalse(IsNullable(typeof(T1)));
            }
        }

#endif

        #endregion
    }
}