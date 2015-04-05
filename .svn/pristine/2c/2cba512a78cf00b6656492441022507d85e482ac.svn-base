using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Perenis.Core.Collections;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Utility pro práci s assemblies.
    /// </summary>
    public static class Assemblies
    {
        /// <summary>
        /// The name of the <c>mscorlib</c> assembly.
        /// </summary>
        public const string Mscorlib = "mscorlib";

        // TODO Improve separation of concerns; make this a non-static class and move the GlobalIgnoredAssemblies out into a policy class

        private static readonly Set<string> globalIgnoredAssemblies = new Set<string>(new[] {Mscorlib});

        /// <summary>
        /// Seznam globálně ignorovaných assembly, například pro vyhledávání typů.
        /// </summary>
        public static Set<string> GlobalIgnoredAssemblies
        {
            get { return globalIgnoredAssemblies; }
        }

        /// <summary>
        /// Gets the short name of an <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is a null reference.</exception>
        public static string GetAssemblyShortName(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            string[] parts = assembly.FullName.Split(new[] {','});
            return parts[0];
        }

        /// <summary>
        /// Poskytne všechny assembly v této aplikační doméně s výjimkou těch uvedených
        /// v <see cref="GlobalIgnoredAssemblies"/> a na dodaném seznamu ignorovaných assembly.
        /// </summary>
        /// <param name="ignoreList">Seznam ignorovaných assembly.</param>
        /// <returns>Seznam všech assembly.</returns>
        public static IEnumerable<Assembly> EnumerateAllAssemblies(Set<string> ignoreList)
        {
            // prohledat všechna assembly
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                // vynechat assembly na globálním black listu
                string shortName = GetAssemblyShortName(a);
                if (!GlobalIgnoredAssemblies.Contains(shortName) && (ignoreList == null || !ignoreList.Contains(shortName)))
                {
                    yield return a;
                }
            }
        }

        /// <summary>
        /// Poskytne všechny assembly v této aplikační doméně s výjimkou těch uvedených
        /// v <see cref="GlobalIgnoredAssemblies"/>.
        /// </summary>
        /// <returns>Seznam všech assembly.</returns>
        public static IEnumerable<Assembly> EnumerateAllAssemblies()
        {
            foreach (Assembly a in EnumerateAllAssemblies(null)) yield return a;
        }

        /// <summary>
        /// Poskytne všechny assembly v této aplikační doméně s výjimkou těch uvedených
        /// v <see cref="GlobalIgnoredAssemblies"/> a na dodaném seznamu ignorovaných assembly.
        /// </summary>
        /// <param name="ignoreList">Seznam ignorovaných assembly.</param>
        /// <returns>Seznam všech assembly.</returns>
        public static List<Assembly> GetAllAssemblies(Set<string> ignoreList)
        {
            var result = new List<Assembly>();
            foreach (Assembly a in EnumerateAllAssemblies(ignoreList)) result.Add(a);
            return result;
        }

        /// <summary>
        /// Poskytne všechny assembly v této aplikační doméně s výjimkou těch uvedených
        /// v <see cref="GlobalIgnoredAssemblies"/>.
        /// </summary>
        /// <returns>Seznam všech assembly.</returns>
        public static List<Assembly> GetAllAssemblies()
        {
            return GetAllAssemblies(null);
        }

        /// <summary>
        /// Retrieves the local storage path of the given assembly.
        /// </summary>
        /// <param name="assembly">An assembly.</param>
        /// <returns>The local storage path of the assembly.</returns>
        public static string GetAssemblyLocalPath(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            // assemblies generated via Reflection.Emit throw NotSupportedException when CodeBase is accessed
            Uri codeBase = assembly is AssemblyBuilder ? null : new Uri(assembly.CodeBase);
            return codeBase != null && !String.IsNullOrEmpty(codeBase.LocalPath) ? codeBase.LocalPath : null;
        }

        /// <summary>
        /// A call to this method forces the CLR to load the assembly that contains the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        public static void PreloadAssembly(Type type)
        {
            string s = type.FullName;
        }
    }
}