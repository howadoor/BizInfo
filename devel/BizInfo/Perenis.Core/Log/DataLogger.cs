using System;
using System.Configuration;
using System.IO;
using System.Text;
using Common.Logging;
using Perenis.Core.Pattern;

namespace Perenis.Core.Log
{
    /// <summary>
    /// Provides a facility for logging large portions of data into separate dump files.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each portion of data is saved into a file whose file name pattern is specified in the 
    /// <c>appSettings</c> configuration section under the key <c>Perenis.Core.Logging.DumpFileName</c>. 
    /// The pattern shall contain both directory and file name indication, and may use the following 
    /// format items:
    /// </para>
    /// <list>
    ///     <item><c>{0}</c> — represents the <see cref="LoggingContextProvider.CurrentLoggingSessionId"/>).</item>
    ///     <item><c>{1}</c> — represents a sequence number, unique for the current logging session.</item>
    ///     <item><c>{2}</c> — represents the <see cref="DateTime.Now"/> value.</item>
    /// </list>
    /// <para>
    /// When no file name pattern is specified, dumping is disabled.
    /// </para>
    /// </remarks>
    public class DataLogger : Singleton<DataLogger>
    {
        // TODO Move the Perenis.Core.Logging.DumpFileName setting into a configuration section.

        /// <summary>
        /// Logs (dumps) the given <paramref name="data"/> into a separate file and returns its name.
        /// </summary>
        /// <param name="source">The source object actually logging the data.</param>
        /// <param name="level">Logging level of this message.</param>
        /// <param name="data">The data to be dumped.</param>
        /// <returns>
        /// The name of dump file or the text "&lt;dump disabled&gt;" in case the logging 
        /// <paramref name="level"/> is not enabled.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Whether the logging <paramref name="level"/> is enabled is evaluated against the supplied
        /// <paramref name="source"/> object.
        /// </para>
        /// </remarks>
        public string LogDump(object source, LogLevel level, string data)
        {
            // make sure this logging level is enabled
            if (!source.IsLoggingEnabled(level) || String.IsNullOrEmpty(dumpFileNamePattern)) return "<dump disabled>";

            // construct dump file name;
            LoggingContext lc = LoggingContextProvider.CurrentLoggingContext;
            string fileName;
            DateTime now = DateTime.Now;
            if (lc != null)
            {
                fileName = String.Format(dumpFileNamePattern, lc.LoggingSessionId, lc.GetSequence(), now);
            }
            else
            {
                fileName = String.Format(dumpFileNamePattern, "GLOBAL", LoggingContextProvider.GetSequence(), now);
            }

            // make sure the target directory exists
            string directory = Path.GetDirectoryName(fileName);
            if (!String.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            // dump the data
            using (var output = new FileStream(fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(output, Encoding.UTF8)) sw.Write(data);
            }

            // return the constructed filename
            return fileName;
        }

        /// <summary>
        /// Input <paramref name="data"/> up to 256 characters, not containtig line feeds, returns
        /// just back, other data logs (dumps) into a separate file and returns its name.
        /// </summary>
        /// <param name="source">The source object actually logging the data.</param>
        /// <param name="level">Logging level of this message.</param>
        /// <param name="data">The data to be dumped.</param>
        /// <returns>
        /// The original data or the name of a dump file. In case the logging <paramref name="level"/>
        /// is disabled, always returns the original data.
        /// </returns>
        /// <remarks>
        /// Whether the logging <paramref name="level"/> is enabled is evaluated against the supplied
        /// <paramref name="source"/> object.
        /// </remarks>
        public string LogDumpShort(object source, LogLevel level, string data)
        {
            return source.IsLoggingEnabled(level) && (data.Length > 256 || data.Contains("\n")) ? LogDump(source, LogLevel.Debug, data) : data;
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Pattern of the dump files' file names.
        /// </summary>
        private static readonly string dumpFileNamePattern = ConfigurationManager.AppSettings["Perenis.Core.Logging.DumpFileName"];

        #endregion
    }
}