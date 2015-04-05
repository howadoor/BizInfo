using System;
using BizInfo.App.Services;
using BizInfo.App.Services.Tools;

namespace BizInfo.WebApp.Controls
{
    public static class InfoViewHelpers
    {
        public static string CreateWhereCommand(string searchText)
        {
            var searchPhrase = searchText == null ? null : searchText.Trim();
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var searchSqlParameter = new EasyFts().ToFtsQuery(searchPhrase);
                return string.Format("WHERE CONTAINS (([Text], [Summary]), '{0}')", searchSqlParameter);
            }
            else
            {
                return null;
            }
        }

        public static string DefaultSelectCommand
        {
            //get { return "WITH OrderedOrders AS (SELECT *, ROW_NUMBER() OVER (ORDER BY CreationTime DESC) AS 'RowNumber' FROM [BizInfo].[dbo].[InfoSetView]) SELECT * FROM OrderedOrders WHERE RowNumber BETWEEN 0 AND 10"; }
            //get { return "SELECT * FROM [BizInfo].[dbo].[InfoSetView] ORDER BY [CreationTime] DESC"; }
            get { return "SELECT * FROM [BizInfo].[dbo].[InfoSetView]"; }
        }

        public static string DefaultOrderCommand
        {
            get { return "ORDER BY CreationTime DESC"; }
        }
    }
}