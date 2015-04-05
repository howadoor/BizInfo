using System;
using System.Xml.Serialization;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Well-known disposable pattern implementation.
    /// </summary>
    /// <remarks>
    /// A disposable type may inherit this class to gain standard implementation of the
    /// <see cref="IDisposable"/> pattern. The <see cref="Dispose(bool)"/> method must be overriden
    /// to perform the actual application-defined tasks associated with proper disposal of the
    /// object. The method is guaranteed to be called only once, either by executing the
    /// <see cref="IDisposable.Dispose"/> method or the class'es finalizer.
    /// </remarks>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Indicates that this instance has been disposed.
        /// </summary>
        [XmlIgnore]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting 
        /// unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> when called on behalf of the 
        /// <see cref="IDisposable.Dispose"/> method; <c>false</c> when called from the class'es
        /// finalizer.</param>
        /// <remarks>
        /// Normally, the implementor of this method shall take any action only when 
        /// <paramref name="disposing"/> is <c>true</c>.
        /// </remarks>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Ensures that this instance hasn't been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">When this instance has been disposed.</exception>
        protected void EnsureNotDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().FullName);
        }

        #region ------ Implementation of the IDisposable interface and pattern --------------------

        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
            IsDisposed = true;
        }

        #endregion
    }
}