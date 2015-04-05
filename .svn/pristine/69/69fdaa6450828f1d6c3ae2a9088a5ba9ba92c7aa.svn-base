using System;
using System.IO;
using System.Text;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides exception text-formatting capabilities.
    /// </summary>
    /// <remarks>
    /// The amount of information produced by an actual exception formatter is implementation-specific.
    /// </remarks>
    public interface IExceptionFormatter
    {
        /// <summary>
        /// Formats the given exception.
        /// </summary>
        /// <param name="ex">The exception to be formatted.</param>
        /// <returns>The text-formatted exception.</returns>
        string Format(Exception ex);

        /// <summary>
        /// Formats the given exception.
        /// </summary>
        /// <param name="destination">The destination into which the exception shall be formatted.</param>
        /// <param name="ex">The exception to be formatted.</param>
        void Format(StringBuilder destination, Exception ex);

        /// <summary>
        /// Formats the given exception.
        /// </summary>
        /// <param name="destination">The destination into which the exception shall be formatted.</param>
        /// <param name="ex">The exception to be formatted.</param>
        void Format(TextWriter destination, Exception ex);
    }
}