using System;
using System.Collections.Generic;
using System.Linq;
using Perenis.Core.Reflection;

namespace Perenis.Core.Decoupling.Multimethods
{
    /// <summary>
    /// Basic implementation of <see cref="IBestMatchingMethodFinder"/> and <see cref="IBestMatchingFunctionFinder"/>.
    /// </summary>
    /// <remarks>
    /// Searching algorithm is based on test of assignability implementor method types to target types and order of implementors in source list.
    /// However it makes no problem now and looks perfectly usable, theoreticaly it can bring unpredictable and unexpected results when used for some combinations
    /// of types.
    /// 
    /// It will be better to replace by better, exactly defined algorithm. Probably based on arguments order.
    /// </remarks>
    internal class BasicBestMatchingMethodFinder : IBestMatchingMethodFinder, IBestMatchingFunctionFinder
    {
        #region IBestMatchingFunctionFinder Members

        public Implementor FindBestMatching(List<Implementor> implementors, Type returnType, Type[] argsTypes)
        {
            Implementor bestMatchingImplementor = null;
            Type[] bestMatchingTypes = null;
            foreach (var implementor in implementors)
            {
                var method = implementor.Method;
                var paramsTypes = method.GetParametersTypes();
                if (!IsAssignableFrom(paramsTypes, argsTypes) || !returnType.IsAssignableFrom(method.ReturnType)) continue;
                if (bestMatchingTypes == null)
                {
                    bestMatchingImplementor = implementor;
                    bestMatchingTypes = paramsTypes;
                }
                else
                {
                    if (IsAssignableFrom(bestMatchingTypes, paramsTypes))
                    {
                        bestMatchingImplementor = implementor;
                        bestMatchingTypes = paramsTypes;
                    }
                }
            }
            return bestMatchingImplementor;
        }

        #endregion

        #region IBestMatchingMethodFinder Members

        public Implementor FindBestMatching(List<Implementor> implementors, Type[] argsTypes)
        {
            Implementor bestMatchingImplementor = null;
            Type[] bestMatchingTypes = null;
            foreach (var implementor in implementors)
            {
                var method = implementor.Method;
                var paramsTypes = method.GetParametersTypes();
                if (!IsAssignableFrom(paramsTypes, argsTypes)) continue;
                if (bestMatchingTypes == null)
                {
                    bestMatchingImplementor = implementor;
                    bestMatchingTypes = paramsTypes;
                }
                else
                {
                    if (IsAssignableFrom(bestMatchingTypes, paramsTypes))
                    {
                        bestMatchingImplementor = implementor;
                        bestMatchingTypes = paramsTypes;
                    }
                }
            }
            return bestMatchingImplementor;
        }

        #endregion

        /// <summary>
        /// Checks if all types from source array can be assigned to target types
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static bool IsAssignableFrom(Type[] target, Type[] source)
        {
            return !source.Where((t, i) => !target[i].IsAssignableFrom(t)).Any();
        }
    }
}