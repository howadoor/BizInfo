using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Provides methods for user-controller setting of input parameters and getting of output parameters.
    /// </summary>
    public interface IParametersAware
    {
        /// <summary>
        /// Indicates if the operation has been provisioned using the <see cref="SetInputParameters"/> method.
        /// </summary>
        bool IsProvisioned { get; }

        /// <summary>
        /// Sets input parameters.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <remarks>
        /// Since calling this method, the <see cref="IsProvisioned"/> property shall return <c>true</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is a null reference.</exception>
        void SetInputParameters(ParameterCollection parameters);

        /// <summary>
        /// Retrieves all output parameters.
        /// </summary>
        /// <returns>The output parameters.</returns>
        ParameterCollection GetOutputParameters();
    }
}