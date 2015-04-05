using System;
using System.Collections.Generic;

namespace Perenis.Core.Decoupling.Multimethods
{
    /// <summary>
    /// Finds best matching implementation of multimethod
    /// </summary>
    public interface IBestMatchingMethodFinder
    {
        /// <summary>
        /// Finds best matching implementation of multimethod
        /// </summary>
        /// <param name="implementors"></param>
        /// <param name="argsTypes"></param>
        /// <returns></returns>
        Implementor FindBestMatching(List<Implementor> implementors, Type[] argsTypes);
    }

    /// <summary>
    /// Finds best matching implementation of multimethod function (method returning something other than <c>void</c>).
    /// </summary>
    public interface IBestMatchingFunctionFinder
    {
        /// <summary>
        /// Finds best matching implementation of multimethod function (method returning something other than <c>void</c>).
        /// </summary>
        /// <param name="implementors"></param>
        /// <param name="returnType"></param>
        /// <param name="argsTypes"></param>
        /// <returns></returns>
        Implementor FindBestMatching(List<Implementor> implementors, Type returnType, Type[] argsTypes);
    }
}