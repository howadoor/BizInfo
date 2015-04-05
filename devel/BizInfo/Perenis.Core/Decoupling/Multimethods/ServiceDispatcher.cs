using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Perenis.Core.General;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.Decoupling.Multimethods
{
    /// <summary>
    /// Multimethod dispatcher used for dispatching methods (returning <c>void</c>).
    /// </summary>
    public class ServiceDispatcher
    {
        /// <summary>
        /// Default binding flags, used when finding methods of services
        /// </summary>
        private static readonly BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// Dispatch table
        /// </summary>
        protected readonly DispatchTable<ServiceMethodInvoker> DispatchTable;

        /// <summary>
        /// Implementors - list of informations about specialized implementations of multimethod
        /// </summary>
        protected readonly List<Implementor> Implementors;

        /// <summary>
        /// Method template
        /// </summary>
        protected readonly MethodInfo MultiMethod;

        /// <summary>
        /// Object responsible to find best matching function from list of implementors
        /// </summary>
        protected IBestMatchingFunctionFinder FunctionFinder;

        /// <summary>
        /// Object responsible to find best matching method from list of implementors
        /// </summary>
        protected IBestMatchingMethodFinder MethodFinder;

        internal ServiceDispatcher(MethodInfo multiMethod)
        {
            Constraints.CheckMultiMethod(multiMethod);
            MultiMethod = multiMethod;
            DispatchTable = CreateDispatchTable(multiMethod);
            Implementors = new List<Implementor>();
            var basicFinder = Singleton<BasicBestMatchingMethodFinder>.Instance; 
            FunctionFinder = basicFinder;
            MethodFinder = basicFinder;
        }

        internal ServiceDispatcher(object[] services, MethodInfo multiMethod)
            : this(multiMethod)
        {
            AddServices(services);
        }

        public int DispatchTableSize
        {
            get { return DispatchTable.Size; }
        }

        protected virtual DispatchTable<ServiceMethodInvoker> CreateDispatchTable(MethodInfo multiMethod)
        {
            var argumentCount = multiMethod.GetParameters().Length;
            return new DispatchTable<ServiceMethodInvoker>(argumentCount, PlatformInfo.Is32BitPlatform());
        }

        /// <summary>
        /// Test candidate method if is allowed to add it to the invokers list
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected virtual bool IsAllowedMethod(MethodInfo candidate)
        {
            return candidate.MethodHandle != MultiMethod.MethodHandle
                   && candidate.Name == MultiMethod.Name
                   && candidate.GetParameters().Length == DispatchTable.ArgumentCount
                   && !candidate.ContainsGenericParameters
                /*&& !HasInterfaceOrAbstractParameter(candidate)*/;
        }

        /// <summary>
        /// Returns a type signature of given method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected virtual Type[] GetMethodTypes(MethodInfo method)
        {
            return method.GetParametersTypes();
        }

        /// <summary>
        /// Finds method conflicting with candidate at methods list
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected virtual MethodInfo FindConflictingMethod(MethodInfo candidate)
        {
            var candidateTypes = GetMethodTypes(candidate);
            return Implementors.FirstOrDefault(implementor =>
                                                   {
                                                       var methodTypes = GetMethodTypes(implementor.Method);
                                                       return methodTypes.SequenceEqual(candidateTypes);
                                                   }).Method;
        }

        /// <summary>
        /// Adds service method to invokers list and to dispatch table
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        protected virtual void AddMethod(object service, MethodInfo method)
        {
            var types = GetMethodTypes(method);
            var invoker = MethodInvokerHelper.CreateServiceInvoker(service, method);
            DispatchTable.InternalAdd(types, invoker);
            Implementors.Add(new Implementor {Method = method, Invoker = invoker});
        }

        /// <summary>
        /// Adds all methods matching multimethod template implemented by service object
        /// </summary>
        /// <param name="service"></param>
        public void AddService(object service)
        {
            if (service == null) return;
            var serviceType = service.GetType();
            Constraints.CheckImplementationType(serviceType);
            var methods = serviceType.GetMethods(DefaultBindingFlags);
            foreach (var candidate in methods)
            {
                try
                {
                    if (IsAllowedMethod(candidate)) AddMethod(service, candidate);
                }
                catch (DispatchTableConflictException dispatchConflict)
                {
                    dispatchConflict.Dispatcher = this;
                    dispatchConflict.AddingMethod = candidate;
                    dispatchConflict.AddingMethodTypes = GetMethodTypes(dispatchConflict.AddingMethod);
                    dispatchConflict.ConflictingMethod = FindConflictingMethod(candidate);
                    dispatchConflict.ConflictingMethodTypes = GetMethodTypes(dispatchConflict.ConflictingMethod);
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds all method implementing specialization of multimethod template from array of objects
        /// </summary>
        /// <param name="services"></param>
        public void AddServices(object[] services)
        {
            foreach (var service in services)
            {
                AddService(service);
            }
        }

        /// <summary>
        /// Creates new service dispatcher based on array of objects exposing methods
        /// </summary>
        /// <param name="services"></param>
        /// <param name="multiMethod">Method template</param>
        /// <returns></returns>
        public static ServiceDispatcher For(object[] services, MethodInfo multiMethod)
        {
            return new ServiceDispatcher(services, multiMethod);
        }

        /// <summary>
        /// Creates new service dispatcher based on array of objects exposing methods
        /// </summary>
        /// <param name="services"></param>
        /// <param name="multiMethod">Method template</param>
        /// <returns></returns>
        public static ServiceDispatcher ForFunction(object[] services, MethodInfo multiMethod)
        {
            return new ServiceFunctionDispatcher(services, multiMethod);
        }

        /// <summary>
        /// Founds best matching method for argument types then calls it and returns result
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DispatchResult Dispatch(params object[] args)
        {
            if (IsAnyArgumentNull(args))
            {
                return DispatchWithNullArgument(args);
            }
            var method = DispatchTable.Match(args);
            if (method != null)
            {
                return InvokeMethod(method, args);
            }
            try
            {
                var implementor = FindBestMatchingImplementor(args);
                method = implementor != null ? implementor.Invoker : delegate { return new DispatchResult(DispatchStatus.NoMatch); };
            }
            catch (AmbiguousMatchException)
            {
                method = delegate { return new DispatchResult(DispatchStatus.AmbiguousMatch); };
            }
            DispatchTable.Add(args, method);
            var result = InvokeMethod(method, args);
            result.IsDynamicInvoke = true;
            return result;
        }

        /// <summary>
        /// Called when one of argument for dispatching is <c>null</c>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual DispatchResult DispatchWithNullArgument(object[] args)
        {
            return new DispatchResult(DispatchStatus.NoMatch);
        }

        /// <summary>
        /// Dispatches function for given arguments a return type
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DispatchResult DispatchFunction(Type returnType, params object[] args)
        {
            if (IsAnyArgumentNull(args))
            {
                return new DispatchResult(DispatchStatus.NoMatch);
            }
            var method = DispatchTable.MatchFunction(returnType, args);
            if (method != null)
            {
                return InvokeMethod(method, args);
            }
            try
            {
                var implementor = FindBestMatchingFunctionImplementor(returnType, args);
                method = implementor != null ? implementor.Invoker : delegate { return new DispatchResult(DispatchStatus.NoMatch); };
            }
            catch (AmbiguousMatchException)
            {
                method = delegate { return new DispatchResult(DispatchStatus.AmbiguousMatch); };
            }
            DispatchTable.AddFunction(returnType, args, method);
            var result = InvokeMethod(method, args);
            result.IsDynamicInvoke = true;
            return result;
        }

        #region Syntactic Sugar

        public static MultiMethod.Action<A1> Action<A1>(Action<A1> action, object[] services)
        {
            var dispatcher = For(services, action.Method);
            return (a1) => dispatcher.Dispatch(a1);
        }

        public static MultiMethod.Action<A1, A2> Action<A1, A2>(Action<A1, A2> action, object[] services)
        {
            var dispatcher = For(services, action.Method);
            return (a1, a2) => dispatcher.Dispatch(a1, a2);
        }

        public static MultiMethod.Action<A1, A2, A3> Action<A1, A2, A3>(Action<A1, A2, A3> action, object[] services)
        {
            var dispatcher = For(services, action.Method);
            return (a1, a2, a3) => dispatcher.Dispatch(a1, a2, a3);
        }

        public static MultiMethod.Action<A1, A2, A3, A4> Action<A1, A2, A3, A4>(Action<A1, A2, A3, A4> action, object[] services)
        {
            var dispatcher = For(services, action.Method);
            return (a1, a2, a3, a4) => dispatcher.Dispatch(a1, a2, a3, a4);
        }

        /// <summary>
        /// TODO: Implement dispatching of paramaterless function
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="func"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static MultiMethod.Func<R> Func<R>(Func<R> func, object[] services)
        {
            throw new NotImplementedException();
            /*
            ServiceDispatcher dispatcher = ServiceDispatcher.ForFunction(services, func.Method);
            return (a1) => dispatcher.DispatchFunction(typeof(R)).Typed<R>();
             */
        }

        public static MultiMethod.Func<A1, R> Func<A1, R>(Func<A1, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (a1) => dispatcher.DispatchFunction(typeof (R), a1).Typed<R>();
        }

        public static MultiMethod.Func<A1, A2, R> Func<A1, A2, R>(Func<A1, A2, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (a1, a2) => dispatcher.DispatchFunction(typeof (R), a1, a2).Typed<R>();
        }

        public static MultiMethod.Func<A1, A2, A3, R> Func<A1, A2, A3, R>(Func<A1, A2, A3, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (a1, a2, a3) => dispatcher.DispatchFunction(typeof (R), a1, a2, a3).Typed<R>();
        }

        /*
        public static MultiMethod.Func<A1, A2, A3, A4, R> Func<A1, A2, A3, A4, R>(Func<A1, A2, A3, A4, R> func, object[] services)
        {
            ServiceDispatcher dispatcher = ServiceDispatcher.ForFunction(services, func.Method);
            return (a1, a2, a3, a4) => dispatcher.DispatchFunction(typeof(R), a1, a2, a3, a4).Typed<R>();
        }
        */

        public static MultiMethod.Function Function<R>(Func<R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return returnType => dispatcher.DispatchFunction(returnType);
        }

        public static MultiMethod.Function<A1> Function<A1, R>(Func<A1, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (returnType, a1) => dispatcher.DispatchFunction(returnType, a1);
        }

        public static MultiMethod.Function<A1, A2> Function<A1, A2, R>(Func<A1, A2, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (returnType, a1, a2) => dispatcher.DispatchFunction(returnType, a1, a2);
        }

        public static MultiMethod.Function<A1, A2, A3> Func<A1, A2, A3, R>(Function<A1, A2, A3, R> func, object[] services)
        {
            var dispatcher = ForFunction(services, func.Method);
            return (returnType, a1, a2, a3) => dispatcher.DispatchFunction(returnType, a1, a2, a3);
        }

        #endregion

        #region Private

        private static DispatchResult InvokeMethod(ServiceMethodInvoker method, object[] args)
        {
            var returnValue = method(args);

            var result = new DispatchResult(DispatchStatus.Success);

            if (returnValue != null && returnValue is DispatchResult) // we registered a NoMatch / AmbiguousMatch
            {
                result = (DispatchResult) returnValue;
            }
            else
            {
                result.ReturnValue = returnValue;
            }

            return result;
        }

        private bool TargetOrArgumentIsNull(object target, object[] args)
        {
            return target == null || IsAnyArgumentNull(args);
        }

        private static bool IsAnyArgumentNull(object[] args)
        {
            return Array.Exists(args, arg => arg == null);
        }

        private static Type[] GetTypes(object[] objects)
        {
            return Array.ConvertAll(objects, @object => @object.GetType());
        }

        /// <summary>
        /// Finds specialized implementation of multimethod best matching arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Implementor FindBestMatchingImplementor(object[] args)
        {
            var argsTypes = GetTypes(args);
            return MethodFinder.FindBestMatching(Implementors, argsTypes);
        }

        /// <summary>
        /// Finds specialized implementation of multimethod best matching return type and argument
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Implementor FindBestMatchingFunctionImplementor(Type returnType, object[] args)
        {
            var argsTypes = GetTypes(args);
            return FunctionFinder.FindBestMatching(Implementors, returnType, argsTypes);
        }

        private static bool HasInterfaceOrAbstractParameter(MethodInfo method)
        {
            return method.GetParameters().Any(p => p.ParameterType.IsInterface || p.ParameterType.IsAbstract);
        }

        #endregion
    }
}