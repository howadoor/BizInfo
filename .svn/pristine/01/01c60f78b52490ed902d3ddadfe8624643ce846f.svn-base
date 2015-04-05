using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using BizInfo.Model.Interfaces;

namespace BizInfo.Model.Entities
{
    public partial class Info : IBizInfo
    {
        #region IBizInfo Members

        public IEnumerable<string> PhotoUrlsList
        {
            get { return PhotoUrls.Split('\r'); }
            set
            {
                var stringBuilder = new StringBuilder();
                foreach (var url in value)
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append('\r');
                    stringBuilder.Append(url);
                }
                PhotoUrls = stringBuilder.ToString();
            }
        }

        public int? NativeCategoryId
        {
            get { return NativeCategory; }
            set { NativeCategory = value; }
        }

        private StructuredContent structuredDocument;
        
        public StructuredContent StructuredContentDocument
        {
            get
            {
                if (structuredDocument != null) return structuredDocument;
                if (string.IsNullOrEmpty(StructuredContent)) return null;
                var root = XElement.Parse(StructuredContent);
                return new StructuredContent(root);
            }
            set
            {
                structuredDocument = value;
                var xml = value == null ? null : value.ToXml();
                StructuredContent = xml;
            }
        }

        #endregion
    }
}