using System;
using System.Reflection;
using Perenis.Core.General;
using Perenis.Core.Reflection;

namespace Perenis.Core.Decoupling.Multimethods
{
    /// <summary>
    /// Multimethod dispatcher used for dispatching functions (methods not returning <c>void</c>)
    /// </summary>
    internal class ServiceFunctionDispatcher : ServiceDispatcher
    {
        public ServiceFunctionDispatcher(object[] services, MethodInfo method)
            : base(services, method)
        {
        }

        /// <summary>
        /// Must create dispatch table for one more argument - return type
        /// </summary>
        /// <param name="multiMethod"></param>
        /// <returns></returns>
        protected override DispatchTable<ServiceMethodInvoker> CreateDispatchTable(MethodInfo multiMethod)
        {
            int argumentCount = multiMethod.GetParameters().Length;
            return new DispatchTable<ServiceMethodInvoker>(argumentCount + 1, PlatformInfo.Is32BitPlatform());
        }

        /// <summary>
        /// Test candidate method if is allowed to add it to the invokers list
        /// </summary>
        /// <param name = "candidate"></param>
        /// <returns></returns>
        protected override bool IsAllowedMethod(MethodInfo candidate)
        {
            return candidate.MethodHandle != MultiMethod.MethodHandle
                   && candidate.Name == MultiMethod.Name
                   && candidate.GetParameters().Length == DispatchTable.ArgumentCount - 1
                   && MultiMethod.ReturnType.IsAssignableFrom(candidate.ReturnType)
                   && !candidate.ContainsGenericParameters;
        }

        /// <summary>
        /// Returns a type signature of given method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected override Type[] GetMethodTypes(MethodInfo method)
        {
            return method.GetFunctionParametersTypes();
        }
    }
}