using System;

namespace Perenis.Core.IO
{
    /// <summary>
    /// Writes the output of commands to the console
    /// </summary>
    public class ConsoleOutputWriter : IOutputWriter
    {
        #region ------ Public methods -------------------------------------------

        /// <summary>
        /// Writes line to the console
        /// </summary>
        /// <param name="line">line of text</param>
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        /// <summary>
        /// Writes line to console
        /// </summary>
        /// <param name="format">format string</param>
        /// <param name="args">parameters</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(String.Format(format, args));
        }

        /// <summary>
        /// Writes empty line to the console
        /// </summary>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        #endregion
    }
}