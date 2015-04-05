using System;
using System.Linq;
using System.Reflection;

namespace Perenis.Core.Decoupling.Multimethods
{
    /// <summary>
    /// Various constraints for multimethod dispatchers
    /// </summary>
    public class Constraints
    {
        public const int MaxParameterCount = 4;

        /// <summary>
        /// Checks method implementation. Throws an exception if checking fails.
        /// </summary>
        /// <param name="method"></param>
        public static void CheckMethod(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length > MaxParameterCount)
            {
                throw new ArgumentException(string.Format("{0}.{1} only upto {2} parameters are supported", method.DeclaringType.Name, method, MaxParameterCount));
            }

            if (method.ContainsGenericParameters)
            {
                throw new ArgumentException(string.Format("{0}.{1} generic methods are not supported", method.DeclaringType.Name, method));
            }

            if (parameters.Any(pi => pi.ParameterType.IsByRef))
            {
                throw new ArgumentException(string.Format("{0}.{1} out and ref arguments are not supported", method.DeclaringType.Name, method));
            }
        }

        /// <summary>
        /// Checks multimethod template. Throws an exception when checking fails.
        /// </summary>
        /// <param name="method"></param>
        public static void CheckMultiMethod(MethodInfo method)
        {
            if (method.GetParameters().Length == 0)
            {
                throw new ArgumentException(string.Format("{0}.{1} multimethods should take atleast one parameter", method.DeclaringType.Name, method));
            }

            if (method.IsStatic) // does it makes sense to support?
            {
                throw new ArgumentException(string.Format("{0}.{1} static multimethods are not supported", method.DeclaringType.Name, method));
            }

            CheckMethod(method);
        }

        /// <summary>
        /// Checks type implementing a method
        /// </summary>
        /// <param name="type"></param>
        public static void CheckImplementationType(Type type)
        {
            if (type.ContainsGenericParameters)
            {
                throw new ArgumentException(string.Format("{0} open generic types are not supported", type));
            }
        }
    }
}