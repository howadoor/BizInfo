using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BizInfo.WebApp.Experiment.DynamicData.PageTemplates
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
            ListView1.SetMetaTable(table, table.GetColumnValuesFromRoute(Context));
            GridDataSource.EntityTypeFilter = table.EntityType.Name;
            GridDataSource.AutoPage = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = table.DisplayName;
            GridDataSource.Include = table.ForeignKeyColumnsNames;

            // Disable various options if the table is readonly
            if (table.IsReadOnly)
            {
                //ListView1.Columns[0].Visible = false;
                ListView1.EnablePersistedSelection = false;
            }
        }

        protected void Label_PreRender(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            DynamicFilter dynamicFilter = (DynamicFilter)label.FindControl("DynamicFilter");
            QueryableFilterUserControl fuc = dynamicFilter.FilterTemplate as QueryableFilterUserControl;
            if (fuc != null && fuc.FilterControl != null)
            {
                label.AssociatedControlID = fuc.FilterControl.GetUniqueIDRelativeTo(label);
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary(ListView1.GetDefaultValues());
            base.OnPreRenderComplete(e);
        }

        protected void DynamicFilter_FilterChanged(object sender, EventArgs e)
        {
            //ListView1.PageIndex = 0;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            /*
            var searchPhrase = SearchPhraseBox.Text;
            string where = null;
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                where = string.Format("CONTAINS (it.Summary, '{0}')", searchPhrase);
            }
            GridDataSource.AutoGenerateWhereClause = false;
            GridDataSource.Where = where;
            ListView1.DataSourceID = null;
            ListView1.DataSource = GridDataSource;
            DataPager1.PagedControlID = null;
            DataPager1.SetPageProperties(0, 10, false);
            ListView1.DataBind();
            DataPager1.PagedControlID = ListView1.UniqueID;
            DataPager1.DataBind();
             */
            GridQueryExtender.DataBind();
        }

        protected string GetNavigationUrl(Int64? webSourceId)
        {
            if (webSourceId == null) return null;
            return ListView1.ToString();
        }
    }
}