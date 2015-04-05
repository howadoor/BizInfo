using System;
using System.Collections.Generic;
using Perenis.Core.Pattern;

namespace Perenis.Core.IO
{
    /// <summary>
    /// Writes the output of commands to the console
    /// </summary>
    public class CompositeOutputWriter : Disposable, IOutputWriter
    {
        /// <summary>
        /// writers collection
        /// </summary>
        private readonly List<IOutputWriter> writers = new List<IOutputWriter>();

        /// <summary>
        /// Adds output writer
        /// </summary>
        /// <param name="writer">writers array</param>
        public void Add(IOutputWriter writer)
        {
            writers.Add(writer);
        }

        /// <summary>
        /// Disposes all disposable writers
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            foreach (IOutputWriter w in writers)
            {
                if (w is IDisposable)
                {
                    (w as IDisposable).Dispose();
                }
            }
        }

        #region ------ Public methods -------------------------------------------

        /// <summary>
        /// Writes line to all writers
        /// </summary>
        /// <param name="line">line of text</param>
        public void WriteLine(string line)
        {
            writers.ForEach(w => w.WriteLine(line));
        }

        /// <summary>
        /// Writes line to all writers
        /// </summary>
        /// <param name="format">format string</param>
        /// <param name="args">parameters</param>
        public void WriteLine(string format, params object[] args)
        {
            string line = String.Format(format, args);
            WriteLine(line);
        }

        /// <summary>
        /// Writes empty line to all writers
        /// </summary>
        public void WriteLine()
        {
            writers.ForEach(w => w.WriteLine());
        }

        #endregion
    }
}