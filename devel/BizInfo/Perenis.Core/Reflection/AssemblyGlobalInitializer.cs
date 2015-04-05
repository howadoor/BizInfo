using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Perenis.Core.Reflection.Configuration;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Handles global initialization of assemblies being loaded within the current domain.
    /// </summary>
    public static class AssemblyGlobalInitializer
    {
        /// <summary>
        /// Indicates that the global initialization has been invoked by the program.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Executes all registered global initializers (if they haven't been executed before).
        /// </summary>
        public static void Initialize()
        {
            // prevent multiple invocation/
            if (IsInitialized) return;

            // let everyone know that global assembly initialization is turned on
            IsInitialized = true;

            // register handler for newly loaded assemblies
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            // run initializers of assemblies loaded so-far
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                InitializeAssembly(assembly);
            }

            // load assemblies intended for pre-loading
            if (AssemblyConfigurationSection.Default != null)
            {
                foreach (AssemblyPreloadElement preload in AssemblyConfigurationSection.Default.PreloadedAssemblies)
                {
                    Assembly.Load(preload.Name);
                }
            }
        }

        /// <summary>
        /// Handles the assembly load event.
        /// </summary>
        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            InitializeAssembly(args.LoadedAssembly);
        }

        /// <summary>
        /// Runs the initializer (if any) of the provided <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">An assembly to be initialized.</param>
        private static void InitializeAssembly(Assembly assembly)
        {
#if DEBUG
            Debug.Assert(!initedAssemblies.ContainsKey(assembly.FullName));
#endif

            var attrs = (AssemblyGlobalInitializerAttribute[]) assembly.GetCustomAttributes(typeof (AssemblyGlobalInitializerAttribute), false);
            if (attrs == null) return;
            for (int i = 0; i < attrs.Length; i++)
            {
                RuntimeHelpers.RunClassConstructor(attrs[i].Initializer.TypeHandle);
            }

#if DEBUG
            initedAssemblies[assembly.FullName] = assembly;
#endif
        }

#if DEBUG
        private static readonly IDictionary<string, object> initedAssemblies = new Dictionary<string, object>();
#endif
    }
}