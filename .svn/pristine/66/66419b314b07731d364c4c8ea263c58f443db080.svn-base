using System;

namespace Perenis.Core
{
    /// <summary>
    /// Introduces type safety into the <see cref="IAsyncResult"/>.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IAsyncResult<TState> : IAsyncResult
    {
        /// <summary>
        /// Type-safe version of <see cref="IAsyncResult.AsyncState"/>.
        /// </summary>
        TState TypedAsyncState { get; }
    }
}