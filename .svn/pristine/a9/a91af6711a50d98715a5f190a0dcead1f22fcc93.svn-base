using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BizInfo.Model.Entities;
using Perenis.Core.Reflection;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Simple implementation of <see cref="IMessageCreator"/>
    /// </summary>
    public class MessageCreator : IMessageCreator
    {
        #region IMessageCreator Members

        public string CreateSubject(Tenant tenant, IEnumerable<Info> info)
        {
            if (info.Count() > 1)
            {
                return string.Format("Nalezeno {0} nových informací (Leonis)", info.Count());
            }
            var theInfo = info.Single();
            var structuredContent = theInfo.StructuredContentDocument;
            var builder = new StringBuilder(theInfo.Summary);
            if (!string.IsNullOrEmpty(structuredContent.Price)) builder.AppendFormat(" * {0}", structuredContent.Price);
            if (!string.IsNullOrEmpty(structuredContent.Location)) builder.AppendFormat(" * {0}", structuredContent.Location);
            builder.Append(" (Leonis)");
            return builder.ToString();
        }

        public string CreateMessage(Tenant tenant, IEnumerable<Info> info)
        {
            var builder = new StringBuilder();
            CreateMessagePrologue(builder, tenant, info);
            CreateMessageContent(builder, tenant, info);
            CreateMessageEpilogue(builder, tenant, info);
            return builder.ToString();
        }

        #endregion

        private void CreateMessageEpilogue(StringBuilder builder, Tenant tenant, IEnumerable<Info> info)
        {
            builder.Append("</body></html>");
        }

        private void CreateMessageContent(StringBuilder builder, Tenant tenant, IEnumerable<Info> info)
        {
            builder.AppendFormat("<h1>Zpráva ze systému Leonis {0}</h1>", DateTime.Now);
            builder.AppendFormat("<p>Nalezeno {0} nových informací.", info.Count());
            if (tenant.MinimumIntervalOfMailInMinutes > 0)
            {
                var nextSearch = DateTime.Now + TimeSpan.FromMinutes(tenant.MinimumIntervalOfMailInMinutes);
                builder.AppendFormat(" Příští vyhledávání nových informací dle vašeho zadání proběhne v {0}. Minimální interval mezi hledáním je nastaven na {1}.</p>", nextSearch, TimeSpan.FromMinutes(tenant.MinimumIntervalOfMailInMinutes));
            }
            builder.Append("</p>");
            int infoIndex = 0;
            foreach (var oneInfo in info)
            {
                builder.AppendFormat("<div class=\"info {0}\">", infoIndex % 2 == 0 ? "even" : "odd");
                CreateMessageContent(builder, tenant, oneInfo);
                builder.Append("</div>");
                infoIndex++;
            }
            // var criteria = XmlSerialization.FromXmlString<SearchingCriteriaModel>(tenant.LastUsedSearchCriteria);
            // builder.AppendFormat("<p><small>{0}<br />{1}<br />{2}</small></p>", criteria, criteria.GetQuery(), Environment.MachineName);
        }

        private void CreateMessageContent(StringBuilder builder, Tenant tenant, Info info)
        {
            var title = GetTitle(info);
            var titleLink = GetTitleLink(info);
            var sourceLink = GetSourceLink(info);
            builder.AppendFormat("<h2><a href=\"{1}\">{0}</a></h2>", title, titleLink);
            builder.AppendFormat("<div class=\"more\">{0}</div>", GetSubtitle(info));
            builder.AppendFormat("<p>{0}</p>", info.Text);
            builder.AppendFormat("<p>Podrobné informace: <a href=\"{0}\">{0}</a><br />", titleLink);
            if (string.IsNullOrEmpty(sourceLink))
            {
                builder.Append("</p>");
            }
            else
            {
                builder.AppendFormat("Zdroj: <a href=\"{0}\">{0}</a></p>", sourceLink);
            }
        }

        /// <summary>
        /// Returns content of the subtitle for <see cref="info"/>. Sub is used in the message. 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetSubtitle(Info info)
        {
            var subtitleBuilder = new StringBuilder();
            var structured = info.StructuredContentDocument;
            if (structured != null && !string.IsNullOrEmpty(structured.Price))
            {
                subtitleBuilder.AppendFormat("<span class=\"creation-time\" style=\"color: Green\">{0}</span> &bull; ", structured.Price);
            }
            subtitleBuilder.AppendFormat("<span class=\"creation-time\">{0}</span>", info.CreationTime);
            return subtitleBuilder.ToString();
        }

        /// <summary>
        /// Returns content of the title for <see cref="info"/>. Title is used in the message.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetTitle(Info info)
        {
            return info.Summary;
        }

        /// <summary>
        /// Returns link (URL) to be used as target address of the link of the <see cref="info"/> title in the message
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        /// <remarks>
        /// Link has and address in the http://bizinfo.dyndns.biz/BizInfo/Base/Detail?id=1099240 format
        /// </remarks>
        private string GetTitleLink(Info info)
        {
            return string.Format("http://bizinfo.dyndns.biz/BizInfo/Base/Detail?id={0}", info.Id);
        }

        private string GetSourceLink(Info info)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var webSource = repository.WebSourceSet.SingleOrDefault(ws => ws.Id == info.WebSourceId);
                return webSource == null ? null : webSource.Url;
            }
        }

        private void CreateMessagePrologue(StringBuilder builder, Tenant tenant, IEnumerable<Info> info)
        {
            builder.Append("<html><head><style>");
            builder.Append(GetStyle());
            builder.Append("</style><body>");
        }

        /// <summary>
        /// Returns CSS style of the HTML message
        /// </summary>
        /// <returns></returns>
        private string GetStyle()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceString("BizInfo.Harvesting.Services.Scripts.message_style.css", Encoding.UTF8);
        }
    }
}