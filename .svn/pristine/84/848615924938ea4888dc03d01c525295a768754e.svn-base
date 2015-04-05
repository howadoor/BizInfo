using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides plain-text formatting of exceptions.
    /// </summary>
    public class TextExceptionFormatter : ExceptionFormatterBase, IExceptionFormatter
    {
        private static readonly TextExceptionFormatter _default = new TextExceptionFormatter();

        private string indent = "    ";

        #region ------ Implementation of the IExceptionFormatter interface ------------------------

        public string Format(Exception ex)
        {
            var sw = new StringWriter();
            Format(sw, ex, 0);
            return sw.ToString();
        }

        public void Format(StringBuilder destination, Exception ex)
        {
            Format(new StringWriter(destination), ex, 0);
        }

        public void Format(TextWriter destination, Exception ex)
        {
            Format(destination, ex, 0);
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Retrieves an indentation string for the given <paramref name="indentLevel"/>.
        /// </summary>
        /// <param name="indentLevel"></param>
        /// <returns></returns>
        private string GetIndentString(int indentLevel)
        {
            if (String.IsNullOrEmpty(Indent)) return String.Empty;
            var sb = new StringBuilder(indentLevel*Indent.Length);
            for (int i = 0; i < indentLevel; i++) sb.Append(Indent);
            return sb.ToString();
        }

        /// <summary>
        /// Writes an indentation string for the given <paramref name="indentLevel"/> into the <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="indentLevel"></param>
        private void WriteIndent(TextWriter destination, int indentLevel)
        {
            if (!String.IsNullOrEmpty(Indent))
            {
                for (int i = 0; i < indentLevel; i++) destination.Write(Indent);
            }
        }

        /// <summary>
        /// Writes an indented line of <paramref name="text"/> into the <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="indentLevel"></param>
        /// <param name="text"></param>
        /// <param name="args"></param>
        private void WriteLine(TextWriter destination, int indentLevel, string text, params object[] args)
        {
            WriteIndent(destination, indentLevel);
            destination.WriteLine(text, args);
        }

        /// <summary>
        /// Formats the given exception.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="ex"></param>
        /// <param name="indentLevel"></param>
        private void Format(TextWriter destination, Exception ex, int indentLevel)
        {
            WriteLine(destination, indentLevel, "ExceptionType: {0}", ex.GetType().AssemblyQualifiedName);
            WriteLine(destination, indentLevel, "Message: {0}", ex.Message);
            WriteLine(destination, indentLevel, "Source: {0}", ex.Source);
            WriteLine(destination, indentLevel, "HelpLink: {0}", ex.HelpLink);
            if (ex.Data.Count > 0)
            {
                WriteLine(destination, indentLevel, "Data:");
                foreach (string key in ex.Data.Keys)
                {
                    WriteLine(destination, indentLevel + 1, "{0}: {1}", key, ex.Data[key]);
                }
            }
            NameValueCollection additionalInfo = GetAdditionalInfo();
            if (additionalInfo.Count > 0)
            {
                WriteLine(destination, indentLevel, "AdditionalInfo:");
                foreach (string key in additionalInfo.AllKeys)
                {
                    WriteLine(destination, indentLevel + 1, "{0}: {1}", key, additionalInfo[key]);
                }
            }
            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                WriteLine(destination, indentLevel, "StackTrace:");
                WriteLine(destination, indentLevel + 1, ex.StackTrace.Replace("\n", "\n" + GetIndentString(indentLevel + 1)));
            }
            else
                WriteLine(destination, indentLevel, "StackTrace: N/A");
            if (ex.InnerException != null)
            {
                WriteLine(destination, indentLevel, "InnerException:");
                Format(destination, ex.InnerException, indentLevel + 1);
            }
        }

        #endregion

        /// <summary>
        /// The default instance of this formatter.
        /// </summary>
        public static TextExceptionFormatter Default
        {
            get { return _default; }
        }

        /// <summary>
        /// The string used for indentation of information.
        /// </summary>
        /// <remarks>
        /// By default, the indentation is set to four space characters.
        /// </remarks>
        public string Indent
        {
            get { return indent; }
            set { indent = value; }
        }
    }
}