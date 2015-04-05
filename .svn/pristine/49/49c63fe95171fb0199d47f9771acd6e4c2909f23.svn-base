using System;

namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown by implementation of <see cref="IItemsAdderProvider.GetItemsAdder"/> when no way hot to created items adder was found.
    /// </summary>
    public class CannotCreateItemsAdderException : Exception
    {
        /// <summary>
        /// Constructs new instance
        /// </summary>
        /// <param name="type">Type for which the items adder construction fails</param>
        public CannotCreateItemsAdderException(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Type for which the items adder construction fails
        /// </summary>
        protected Type Type
        {
            get; private set;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get
            {
                return string.Format("Cannot create items adder for {0}", Type);
            }
        }
    }
}