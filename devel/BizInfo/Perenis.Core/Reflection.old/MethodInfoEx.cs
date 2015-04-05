using System;
using System.Reflection;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Extending <c>MethodInfo</c> class
    /// </summary>
    public static class MethodInfoEx
    {
        /// <summary>
        /// Returns array of parameters types
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type[] GetParametersTypes(this MethodInfo method)
        {
            return Array.ConvertAll(method.GetParameters(), parameter => parameter.ParameterType);
        }

        /// <summary>
        /// Returns array of return type (with index 0] and parameter types (type of firts parameter has index 1)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type[] GetFunctionParametersTypes(this MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            var types = new Type[parameters.Length + 1];
            types[0] = method.ReturnType;
            int i = 1;
            foreach (var parameter in parameters)
            {
                types[i] = parameter.ParameterType;
                i++;
            }
            return types;
        }
    }
}