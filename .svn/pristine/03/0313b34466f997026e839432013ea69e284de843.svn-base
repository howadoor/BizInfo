using System;
using System.Reflection;
using Perenis.Core.Reflection;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Exception manipulation helpers.
    /// </summary>
    public static class ExceptionEx
    {
        /// <summary>
        /// Unboxes the original exception from a wrapper of type <typeparamref name="T"/>, 
        /// or returns the original exception.
        /// </summary>
        /// <param name="ex">The exception to be processed.</param>
        /// <typeparam name="T">The type of the wrapping exception.</typeparam>
        /// <returns>The first non-<typeparamref name="T"/> exception found in the inner-exception
        /// stack, or the original exception when not a <typeparamref name="T"/> exception.</returns>
        /// <remarks>
        /// <para>
        /// Note that this method makes sure both the outer and the inner exception receive the same 
        /// handling instance ID.
        /// </para>
        /// <para>
        /// This method is especially useful for unboxing <see cref="TargetInvocationException"/>s.
        /// </para>
        /// </remarks>
        public static Exception Unbox<T>(this Exception ex) where T : Exception
        {
            // unbox the inner exception
            Exception unboxed = ex;
            while (unboxed is T && unboxed.InnerException != null) unboxed = unboxed.InnerException;

            // take over the handling ID
            if (unboxed != null) ExceptionHandler.SetHandlingInstanceId(unboxed, ex.GetHandlingInstanceId());

            return unboxed;
        }

        /// <summary>
        /// Unboxes the original exception from a wrapper of type <typeparamref name="T"/> 
        /// returning a new wrapper exception of the same type as the unboxed exception, having the
        /// original exception as inner exception, or returns the original exception.
        /// </summary>
        /// <param name="ex">The exception to be processed.</param>
        /// <typeparam name="T">The type of the wrapping exception.</typeparam>
        /// <returns>Wrapper of the given exception based on the type of the first 
        /// non-<typeparamref name="T"/> exception found in the inner-exception stack, or the 
        /// original exception when not a <typeparamref name="T"/> exception or when wrapper
        /// construction fails.</returns>
        /// <remarks>
        /// <para>
        /// Note that this method makes sure all the original outer, the original inner, and the new 
        /// wrapper exception receive the same handling instance ID.
        /// </para>
        /// <para>
        /// For the wrapping to succeed, the unboxed exception must have a public constructor accepting
        /// a <see cref="string"/> argument as the exception message, and a <see cref="Exception"/>
        /// argument as the inner exception. Otherwise, wrapping fails and the original 
        /// <typeparamref name="T"/> exception is returned.
        /// </para>
        /// <para>
        /// This method is especially useful for re-wrapping <see cref="TargetInvocationException"/>s.
        /// </para>
        /// </remarks>
        public static Exception UnboxAndWrap<T>(this Exception ex) where T : Exception
        {
            return ex.UnboxAndWrap<T>(false);
        }

        /// <summary>
        /// Unboxes the original exception from a wrapper of type <typeparamref name="T"/> 
        /// returning a new wrapper exception of the same type as the unboxed exception, having the
        /// original exception as inner exception, or returns the original exception.
        /// </summary>
        /// <param name="ex">The exception to be processed.</param>
        /// <param name="wrapAlways">Whether to create a wrapper exception even if the given
        /// exception is not a <typeparamref name="T"/> exception.</param>
        /// <returns>Wrapper of the given exception based on the type of the first 
        /// non-<typeparamref name="T"/> exception found in the inner-exception stack or when
        /// <paramref name="wrapAlways"/> is <c>true</c>, or the original exception when not 
        /// a <typeparamref name="T"/> exception or when wrapper construction fails.</returns>
        /// <remarks>
        /// <para>
        /// Note that this method makes sure all the original outer, the original inner, and the new 
        /// wrapper exception receive the same handling instance ID.
        /// </para>
        /// <para>
        /// For the wrapping to succeed, the unboxed exception must have a public constructor accepting
        /// a <see cref="string"/> argument as the exception message, and a <see cref="Exception"/>
        /// argument as the inner exception. Otherwise, wrapping fails and the original 
        /// <typeparamref name="T"/> exception is returned.
        /// </para>
        /// <para>
        /// This method is especially useful for re-wrapping <see cref="TargetInvocationException"/>s.
        /// </para>
        /// </remarks>
        public static Exception UnboxAndWrap<T>(this Exception ex, bool wrapAlways) where T : Exception
        {
            // unbox the orignal exception
            Exception unboxed = ex.Unbox<T>();
            Exception wrapped = null;

            // in case unboxing led to a meaningful result, create a wrapper expcetion to save original call stacks
            if (unboxed != null && (wrapAlways || unboxed != ex))
            {
                try
                {
                    // create wrapper
                    wrapped = TypeFactory<Exception>.Instance.CreateInstance(unboxed.GetType(), unboxed.Message, ex);

                    // mark it as a wrapper
                    wrapped.Data["Wrapper"] = true;

                    // take over the handling ID
                    ExceptionHandler.SetHandlingInstanceId(wrapped, unboxed.GetHandlingInstanceId());
                }
                catch (Exception)
                {
                    // all exceptions thrown from wrapper construction are ignored, causing the original 
                    // unmodified exception to be returned ⇒ this way the full stack-trace is always preserved
                }
            }
            return wrapped ?? ex;
        }

        /// <summary>
        /// Checks if the given <paramref name="exception"/> is a wrapper over an original exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns><c>true</c> if the exception is a wrapper; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is a null reference.</exception>
        public static bool IsWrapper(this Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            return exception.Data.Contains("Wrapper") && (bool) exception.Data["Wrapper"];
        }
    }
}