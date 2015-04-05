using System;
using System.Collections.Generic;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// Represents a root command-line operation.
    /// </summary>
    public interface IRootCmdOperation : ICmdOperation
    {
        /// <summary>
        /// List of inner operations
        /// </summary>
        List<ICmdOperation> InnerOperations { get; set; }

        /// <summary>
        /// List of known operation types
        /// </summary>
        Dictionary<Type, CmdOperationAttribute> KnownOperationTypes { get; set; }

        /// <summary>
        /// Executes commands.
        /// </summary>
        void ExecuteOperations();
    }
}