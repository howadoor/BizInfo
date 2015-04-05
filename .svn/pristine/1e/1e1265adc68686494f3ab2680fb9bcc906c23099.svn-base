using System;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Represents a type whose static constructor shall be executed as a global initializer of an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyGlobalInitializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="initializer">The type whose static constructor performs global assembly initialization.</param>
        public AssemblyGlobalInitializerAttribute(Type initializer)
        {
            if (initializer.IsGenericTypeDefinition) throw new ArgumentException("The initializer type may not be a generic type definition.", "initializer");
            // NOTE There's no actual reason to exclude abstract types from the global initialization mechanism.
            // if (!initializer.IsSealed && initializer.IsAbstract) throw new ArgumentException("The initializer type may not be abstract.", "initializer");
            if (initializer.TypeInitializer == null) throw new ArgumentException("The initializer must have a static constructor.", "initializer");
            Initializer = initializer;
        }

        /// <summary>
        /// The type whose static constructor performs global assembly initialization.
        /// </summary>
        public Type Initializer { get; set; }
    }
}