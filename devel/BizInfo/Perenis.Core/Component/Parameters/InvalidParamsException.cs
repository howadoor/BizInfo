using System;
using System.Runtime.Serialization;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Thrown when the value of a parameter supplied to an object is invalid.
    /// </summary>
    public class InvalidParamsException : Exception
    {
        #region ------ Constructors ---------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public InvalidParamsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        public InvalidParamsException(string message, params object[] args)
            : base(args != null ? String.Format(message, args) : message)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        public InvalidParamsException(Exception innerException, string message, params object[] args)
            : base(args != null ? String.Format(message, args) : message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        public InvalidParamsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}