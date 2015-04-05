using System;
using System.Runtime.Serialization;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Thrown when one or more parameters required by an object are missing in the input.
    /// </summary>
    public class MissingParamsException : Exception
    {
        #region ------ Constructors ---------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public MissingParamsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message.
        /// </summary>
        public MissingParamsException(string message, params object[] args)
            : base(args != null ? String.Format(message, args) : message)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        public MissingParamsException(Exception innerException, string message, params object[] args)
            : base(args != null ? String.Format(message, args) : message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with serialized data.
        /// </summary>
        public MissingParamsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}