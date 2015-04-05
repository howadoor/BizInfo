using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using BizInfo.WebApp.MVC3.Data;

namespace BizInfo.WebApp.MVC3.Controls
{
    public partial class InfoView : System.Web.UI.UserControl
    {
        [Category("Behavior")]
        [Description("Total number of records")]
        [DefaultValue(null)]
        public string SearchPhrase
        {
            get
            {
                var o = ViewState["SearchPhrase"];
                if (o == null) return null;
                return (string) o;
            }
            set { ViewState["SearchPhrase"] = value; }

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustomDataPager1.TotalRecords = GetTotalRowsCount();
            }
            CustomDataPager1.UpdatePageIndex = UpdateListViewContent;
        }

        private int GetTotalRowsCount()
        {
            using (SqlConnection connection = new SqlConnection(SqlDataSource.ConnectionString))
            {
                connection.Open();
                var searchBase = InfoViewHelpers.DefaultSelectCommand;
                var whereCommand = InfoViewHelpers.CreateWhereCommand(SearchPhrase, null);
                var search = string.Format("WITH q AS ({0} {1}) SELECT COUNT(*) FROM q", searchBase, whereCommand);
                using (SqlCommand command = new SqlCommand(search, connection))
                {
                    return (int) command.ExecuteScalar();
                }
            }
        }

        protected void Page_Init()
        {
            // SqlDataSource.SelectCommand = InfoViewHelpers.DefaultSelectCommand;
        }

        private void UpdateListViewContent(int startRow, int endRow)
        {
            var whereCommand = InfoViewHelpers.CreateWhereCommand(SearchPhrase, null);
            var orderCommand = InfoViewHelpers.DefaultOrderCommand;
            var pageCommand = string.Format("WITH OrderedOrders AS (SELECT *, ROW_NUMBER() OVER ({1}) AS 'RowNumber' FROM [BizInfo].[dbo].[Infoset] {0}) SELECT * FROM OrderedOrders WHERE RowNumber BETWEEN {2} AND {3}", whereCommand, orderCommand, startRow, endRow);
            SqlDataSource.SelectCommand = pageCommand;
            ListView1.DataBind();
        }

        protected string GetWebSourceNavigation(Int64? webSourceId)
        {
            if (!webSourceId.HasValue) return null;
            var webSource = DataSource.Container.WebSourceSet.Where(ws => ws.Id == webSourceId.Value).FirstOrDefault();
            if (webSource == null || string.IsNullOrEmpty(webSource.Url)) return null;
            var uri = new Uri(webSource.Url);
            return string.Format("<a class=\"web-source-link\" href=\"{0}\">{1}</a>", webSource.Url, GetServerOnly(uri.Host));
        }

        private string GetServerOnly(string host)
        {
            var lastPoint = host.LastIndexOf('.');
            var prevPoint = host.LastIndexOf('.', lastPoint - 1, lastPoint);
            if (prevPoint < 0) return host;
            return host.Substring(prevPoint + 1);
        }

        protected string GetCategoryString(int? categoryId)
        {
            if (!categoryId.HasValue) return null;
            var category = DataSource.Container.CategorySet.Where(cat => cat.Id == categoryId.Value).FirstOrDefault();
            if (category == null) return null;
            return category.Name;
        }

        protected string ProcessText(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var mailMatches = Regex.Matches(text, @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);
            if (mailMatches.Count == 0) return ProcessPhones(text);
            var stringBuilder = new StringBuilder();
            int lastStart = 0;
            foreach (var mailMatch in mailMatches.Cast<Match>())
            {
                stringBuilder.Append(ProcessPhones(text.Substring(lastStart, mailMatch.Index - lastStart)));
                stringBuilder.AppendFormat("<span class=\"mail\"><a title=\"Napsat mail na adresu {0}\" href=\"mailto:{0}\">{0}</a><a href=\"http://www.google.cz/search?q={0}\" title=\"Vyhledat {0} Googlem\">{1}</a></span>", mailMatch.Value, GoogleIcon);
                lastStart = mailMatch.Index + mailMatch.Length;
            }
            stringBuilder.Append(ProcessPhones(text.Substring(lastStart)));
            return stringBuilder.ToString();
        }

        private string ProcessPhones(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var mailMatches = Regex.Matches(text, @"\d\s*\d\s*\d\s*\d\s*\d\s*\d\s*\d\s*\d\s*\d\s*", RegexOptions.IgnoreCase);
            if (mailMatches.Count == 0) return ProcessUrl(text);
            var stringBuilder = new StringBuilder();
            int lastStart = 0;
            foreach (var mailMatch in mailMatches.Cast<Match>())
            {
                stringBuilder.Append(ProcessUrl(text.Substring(lastStart, mailMatch.Index - lastStart)));
                stringBuilder.AppendFormat("<span class=\"phone\">{0}<a href=\"http://www.google.cz/search?q={1}\" title=\"Vyhledat {0} Googlem\">{2}</a></span>", mailMatch.Value, GetPhoneSearchPhrase(mailMatch.Value), GoogleIcon);
                lastStart = mailMatch.Index + mailMatch.Length;
            }
            stringBuilder.Append(ProcessUrl(text.Substring(lastStart)));
            return stringBuilder.ToString();
        }

        private string ProcessUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var mailMatches = Regex.Matches(text, @"\b(?:(?:https?|ftp|file)://|www\.|ftp\.)(?:\([-A-Z0-9+&@#/%=~_|?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])", RegexOptions.IgnoreCase);
            if (mailMatches.Count == 0) return text;
            var stringBuilder = new StringBuilder();
            int lastStart = 0;
            foreach (var mailMatch in mailMatches.Cast<Match>())
            {
                stringBuilder.Append(text.Substring(lastStart, mailMatch.Index - lastStart));
                stringBuilder.AppendFormat("<span class=\"mail\"><a title=\"Přejít na adresu {0}\" href=\"{0}\">{1}</a><a href=\"http://www.google.cz/search?q={1}\" title=\"Vyhledat {0} Googlem\">{2}</a></span>", GetTargetUrl(mailMatch.Value), mailMatch.Value, GoogleIcon);
                lastStart = mailMatch.Index + mailMatch.Length;
            }
            stringBuilder.Append(text.Substring(lastStart));
            return stringBuilder.ToString();
        }

        private string GetTargetUrl(string partialUrl)
        {
            if (partialUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) return partialUrl;
            return "http://" + partialUrl;
        }

        protected string GoogleIcon
        {
            get { return "<img class=\"google-icon\" src=\"http://www.google.com/favicon.ico\" />"; }
        }

        private string GetPhoneSearchPhrase(string _phone)
        {
            var phone = _phone.Replace(" ", string.Empty);
            return string.Format("&quot;{0} {1} {2}&quot; OR {3}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6, 3), phone);
        }

        protected void OnObjectSourceQueryCreated(object sender, EventArgs e)
        {
        }

        protected void OnSearchPhraseTextBoxChanged(object sender, EventArgs e)
        {
           SearchPhrase = SearchPhraseBox.Text;
           CustomDataPager1.UpdatePaging(1, CustomDataPager1.RecordsPerPage, GetTotalRowsCount());
        }

        protected void OnSqlDataSourceSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 60;
        }

        protected string GetBizInfoImage(string urlsString)
        {
            if (string.IsNullOrEmpty(urlsString)) return "<div class=\"item-image\"></div>";
            var urls = urlsString.Split('\r');
            return string.Format("<img class=\"item-image\" src=\"{0}\"/>", urls.FirstOrDefault());
        }
    }
}