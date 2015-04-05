using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides XML formatting of exceptions.
    /// </summary>
    public class XmlExceptionFormatter : ExceptionFormatterBase, IExceptionFormatter
    {
        private static readonly XmlExceptionFormatter _default = new XmlExceptionFormatter();

        private XmlWriterSettings settings = new XmlWriterSettings {Indent = true, IndentChars = "    ", OmitXmlDeclaration = true, CheckCharacters = false};

        /// <summary>
        /// The default instance of this formatter.
        /// </summary>
        public static XmlExceptionFormatter Default
        {
            get { return _default; }
        }

        /// <summary>
        /// The XML writing settings.
        /// </summary>
        /// <remarks>
        /// By default, XML declaration is omitted and indentation is set to four spaces.
        /// </remarks>
        public XmlWriterSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        /// <summary>
        /// Formats the given exception.
        /// </summary>
        /// <param name="destination">The destination into which the exception shall be formatted.</param>
        /// <param name="ex">The exception to be formatted.</param>
        public void Format(XmlWriter destination, Exception ex)
        {
            destination.WriteStartElement("Exception");
            WriteElement(destination, "ExceptionType", ex.GetType().AssemblyQualifiedName);
            WriteElement(destination, "Message", ex.Message);
            WriteElement(destination, "Source", ex.Source);
            WriteElement(destination, "HelpLink", ex.HelpLink);
            if (ex.Data.Count > 0)
            {
                destination.WriteStartElement("Data");
                foreach (string key in ex.Data.Keys)
                {
                    WriteElement(destination, key, ex.Data[key].ToString());
                }
                destination.WriteEndElement();
            }
            NameValueCollection additionalInfo = GetAdditionalInfo();
            if (additionalInfo.Count > 0)
            {
                destination.WriteStartElement("AdditionalInfo");
                foreach (string key in additionalInfo.AllKeys)
                {
                    WriteElement(destination, key, additionalInfo[key]);
                }
                destination.WriteEndElement();
            }
            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                WriteElement(destination, "StackTrace", ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                destination.WriteStartElement("InnerException");
                Format(destination, ex.InnerException);
                destination.WriteEndElement();
            }
            destination.WriteEndElement();
        }

        #region ------ Implementation of the IExceptionFormatter interface ------------------------

        public string Format(Exception ex)
        {
            var sw = new StringWriter();
            Format(sw, ex);
            return sw.ToString();
        }

        public void Format(StringBuilder destination, Exception ex)
        {
            using (XmlWriter xw = XmlWriter.Create(destination, Settings))
            {
                Format(xw, ex);
                xw.Flush();
            }
        }

        public void Format(TextWriter destination, Exception ex)
        {
            using (XmlWriter xw = XmlWriter.Create(destination, Settings))
            {
                Format(xw, ex);
                xw.Flush();
            }
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        private void WriteElement(XmlWriter destination, string tag, string value)
        {
            destination.WriteStartElement(tag);
            destination.WriteString(value);
            destination.WriteEndElement();
        }

        #endregion
    }
}