using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using BizInfo.Harvesting.Services.Processing.Fragments;

namespace BizInfo.WebApp.MVC3.Tools
{
    public static class HtmlHelperTools
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            var output = new StringBuilder();
            output.Append("<div class=\"checkboxList\">");
            foreach (var item in items)
            {
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");
                if (item.Selected) output.Append(" checked=\"checked\"");
                output.Append(" />");
                output.Append(item.Text);
                output.Append("<br />");
            }
            output.Append("</div>");
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString ProcessText(this HtmlHelper helper, string text, int maxLength = 0)
        {
            var cutted = maxLength > 0 && text.Length > maxLength;
            if (cutted) text = text.Substring(0, maxLength);
            var html = TextProcessingTools.Processor.Process(text);
            if (cutted) html += "&hellip;";
            return MvcHtmlString.Create(html);
        }
    }
}