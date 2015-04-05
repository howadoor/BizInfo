using Perenis.Core.Collections;
using Perenis.Core.Component.Progress;
using Perenis.Core.IO;

namespace Perenis.Core.CommandLine
{
    /// <summary>
    /// Represents a command-line operation.
    /// </summary>
    public interface ICmdOperation
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <param name="objectStack">Object stack used to comunicate between commands</param>
        /// <param name="progressObserver">Progress observer</param>
        /// <param name="output">Command output writer</param>
        void ExecuteOperation(StackWithOptionalNames objectStack, IOutputWriter output, IProgressObserver progressObserver);
    }
}