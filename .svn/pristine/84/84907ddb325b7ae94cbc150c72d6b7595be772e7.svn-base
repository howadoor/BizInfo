using System;
using System.Globalization;
using System.Reflection;
using Perenis.Core.Exceptions;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// <see cref="MethodInfo"/> manipulation helpers.
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
            foreach (ParameterInfo parameter in parameters)
            {
                types[i] = parameter.ParameterType;
                i++;
            }
            return types;
        }

        /// <summary>
        /// Calls the <see cref="MethodBase.Invoke(object,object[])"/> method and in case 
        /// a <see cref="TargetInvocationException"/> is thrown, rethrows the unboxed and wrapped
        /// real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <remarks>
        /// See <see cref="MethodBase.Invoke(object,object[])"/> for more details on the semantics
        /// of this method and its parameters.
        /// </remarks>
        public static object InvokeUnbox(this MethodInfo method, object obj, object[] parameters)
        {
            try
            {
                return method.Invoke(obj, parameters);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }

        /// <summary>
        /// Calls the <see cref="MethodBase.Invoke(object,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo)"/> 
        /// method and in case a <see cref="TargetInvocationException"/> is thrown, rethrows the 
        /// unboxed and wrapped real inner exception using <see cref="ExceptionEx.UnboxAndWrap"/>.
        /// </summary>
        /// <remarks>
        /// See <see cref="MethodBase.Invoke(object,object[])"/> for more details on the semantics
        /// of this method and its parameters.
        /// </remarks>
        public static object InvokeUnbox(this MethodInfo method, object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            try
            {
                return method.Invoke(obj, invokeAttr, binder, parameters, culture);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.UnboxAndWrap<TargetInvocationException>();
            }
        }
    }
}