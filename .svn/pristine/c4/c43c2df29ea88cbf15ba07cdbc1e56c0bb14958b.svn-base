using System;
using System.IO;
using Perenis.Core.Pattern;

namespace Perenis.Core.IO
{
    /// <summary>
    /// Writes the output of commands to the console
    /// </summary>
    public class FileOutputWriter : Disposable, IOutputWriter
    {
        /// <summary>
        /// File where output is written to
        /// </summary>
        private StreamWriter file;

        /// <summary>
        /// Construcor
        /// Opens the file for output
        /// </summary>
        /// <param name="fileFullName">file full name</param>
        /// <param name="append">if file is overwritten or not</param>
        public FileOutputWriter(string fileFullName, bool append)
        {
            file = new StreamWriter(fileFullName, append);
        }

        #region ------ Public methods -------------------------------------------

        /// <summary>
        /// Writes line to the console
        /// </summary>
        /// <param name="line">line of text</param>
        public void WriteLine(string line)
        {
            file.WriteLine(line);
        }

        /// <summary>
        /// Writes line to console
        /// </summary>
        /// <param name="format">format string</param>
        /// <param name="args">parameters</param>
        public void WriteLine(string format, params object[] args)
        {
            file.WriteLine(String.Format(format, args));
        }

        /// <summary>
        /// Writes empty line to the console
        /// </summary>
        public void WriteLine()
        {
            file.WriteLine();
        }

        #endregion

        /// <summary>
        /// close the output file
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && file != null)
            {
                file.Flush();
                file.Close();
                file = null;
            }
        }
    }
}