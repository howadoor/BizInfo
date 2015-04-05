using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Security;
using BizInfo.App.Services.Logging;
using BizInfo.App.Services.Tools;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Models.Base;
using BizInfo.WebApp.MVC3.Models.Common;
using Perenis.Core.Serialization;
using SearchingCriteriaModelEx = BizInfo.WebApp.MVC3.Models.Base.SearchingCriteriaModelEx;

namespace BizInfo.WebApp.MVC3.Tools
{
    /// <summary>
    /// TODO Convert to parametrized query!!!
    /// </summary>
    public static class SearchingTools
    {
        private static string DefaultSelectCommand
        {
            //get { return "WITH OrderedOrders AS (SELECT *, ROW_NUMBER() OVER (ORDER BY CreationTime DESC) AS 'RowNumber' FROM [BizInfo].[dbo].[InfoSetView]) SELECT * FROM OrderedOrders WHERE RowNumber BETWEEN 0 AND 10"; }
            //get { return "SELECT * FROM [BizInfo].[dbo].[InfoSetView] ORDER BY [CreationTime] DESC"; }
            get { return "SELECT * FROM [BizInfo].[dbo].[InfoSetView]"; }
        }

        private static string DefaultOrderCommand
        {
            get { return "ORDER BY CreationTime DESC"; }
        }

        public static BizInfoModel[] GetPageItems(this PagingModel pagingModel)
        {
            Exception exceptionOccured = null;
            var startTime = DateTime.Now;
            var itemsFound = 0;
            try
            {
                var items = DoGetPageItems(pagingModel);
                itemsFound = items.Length;
                return items;
            }
            catch (Exception exception)
            {
                exceptionOccured = exception;
                throw;
            }
            finally
            {
                var duration = DateTime.Now - startTime;
                LogSearching(pagingModel, startTime, duration, itemsFound, exceptionOccured);
            }
        }

        private static void LogSearching(PagingModel pagingModel, DateTime startTime, TimeSpan duration, int itemsFound, Exception exceptionOccured)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var criteria = GetSearchingCriteria(pagingModel, repository);
                criteria.LastUsedTime = DateTime.Now;
                criteria.UsageCount++;
                var searchingLog = repository.SearchingLogSet.CreateObject();
                searchingLog.SearchCriteriaId = criteria.Id;
                if (exceptionOccured != null) searchingLog.Exception = exceptionOccured.ToString();
                searchingLog.Start = startTime;
                searchingLog.Duration = duration;
                searchingLog.ResultsCount = itemsFound;
                searchingLog.UserId = LoggedTenant.Id;
                searchingLog.Succeeded = exceptionOccured == null;
                repository.SearchingLogSet.AddObject(searchingLog);
                repository.SaveChanges();
            }
        }

        private static SearchCriteria GetSearchingCriteria(PagingModel pagingModel, BizInfoModelContainer repository)
        {
            var guid = XmlSerialization.HashToGuid(pagingModel);
            var searchCriteria = repository.SearchCriteriaSet.SingleOrDefault(sc => sc.Hash == guid);
            if (searchCriteria == null)
            {
                searchCriteria = repository.SearchCriteriaSet.CreateObject();
                searchCriteria.Hash = guid;
                searchCriteria.Criteria = pagingModel.ToXmlStringByDataContractSerializer();
                searchCriteria.LastUsedTime = DateTime.Now;
                repository.SearchCriteriaSet.AddObject(searchCriteria);
            }
            return searchCriteria;
        }

        private static BizInfoModel[] DoGetPageItems(PagingModel pagingModel)
        {
            var pageCommand = GetPageCommand(pagingModel);
            // Do not use "using" here because container is used in models retrieved by query
            var container = new BizInfoModelContainer {CommandTimeout = 120};
            using (pageCommand.LogOperation(pageCommand))
            {
                return container.ExecuteStoreQuery<Info>(pageCommand).Select(bizInfo => new BizInfoModel {BizInfo = bizInfo, Container = container}).ToArray();
            }
        }

        public static string GetPageCommand(PagingModel pagingModel)
        {
            return GetPageCommandQuick(pagingModel);
        }

        public static string GetPageCommandQuick(PagingModel pagingModel)
        {
            var whereCommand = CreateWhereCommand(pagingModel);
            var orderCommand = DefaultOrderCommand;
            return
                string.Format(
                    "WITH OrderedOrders AS (SELECT TOP({3}) *, ROW_NUMBER() OVER ({1}) AS 'RowNumber' FROM (SELECT TOP({4}) * FROM [BizInfo].[dbo].[InfoSetView] {0} ORDER BY Id DESC) ZZZ) SELECT * FROM OrderedOrders WHERE RowNumber BETWEEN {2} AND {3} ORDER BY [RowNumber] ASC",
                    whereCommand, orderCommand, pagingModel.StartIndex + 1, pagingModel.EndIndex, 1001);
        }

        public static string GetPageCommandCorrect(PagingModel pagingModel)
        {
            var whereCommand = CreateWhereCommand(pagingModel);
            var orderCommand = DefaultOrderCommand;
            return string.Format("WITH OrderedOrders AS (SELECT TOP({3}) *, ROW_NUMBER() OVER ({1}) AS 'RowNumber' FROM [BizInfo].[dbo].[InfoSetView] {0}) SELECT * FROM OrderedOrders WHERE RowNumber BETWEEN {2} AND {3} ORDER BY [RowNumber] ASC", whereCommand,
                                 orderCommand, pagingModel.StartIndex + 1, pagingModel.EndIndex);
        }

        public static int GetCount(this SearchingCriteriaModel searchingCriteria)
        {
            using (var container = new BizInfoModelContainer {CommandTimeout = 120})
            {
                var query = GetCountQuery(searchingCriteria);
                using (searchingCriteria.LogOperation(query))
                {
                    var result = container.ExecuteStoreQuery<int>(query).Single();
                    searchingCriteria.LogInfo(string.Format("Result = {0}", result));
                    return result;
                }
            }
        }

        public static string GetCountQuery(this SearchingCriteriaModel searchingCriteria)
        {
            return GetCountQuery(searchingCriteria.Phrase, searchingCriteria.NotAllowedPhrase, searchingCriteria.Sources, searchingCriteria.VerbKinds, searchingCriteria.Verbs, searchingCriteria.Domains, searchingCriteria.MaxAge,
                                 searchingCriteria.Important, searchingCriteria.Company, searchingCriteria.CompanyLimit, searchingCriteria.HasContact, searchingCriteria.MinId);
        }

        public static string GetCountQuery(string searchText, string notAllowedText, IEnumerable<int> sources, IEnumerable<int> verbKinds, IEnumerable<int> verbs, IEnumerable<int> domains, int maxAge, bool? importancy, bool? company, int companyLimit,
                                           bool? hasContact, long minId)
        {
            var whereCommand = CreateWhereCommand(searchText, notAllowedText, sources, verbKinds, verbs, domains, maxAge, importancy, company, companyLimit, hasContact, minId);
            return string.Format("WITH q AS (SELECT TOP(1001) * FROM [BizInfo].[dbo].[InfoSetView] {0}) SELECT COUNT(*) FROM q", whereCommand);
        }

        public static string GetExactCountQuery(string searchText, string notAllowedText, IEnumerable<int> sources, IEnumerable<int> verbKinds, IEnumerable<int> verbs, IEnumerable<int> domains, int maxAge, bool? importancy, bool? company, int companyLimit,
                                                bool? hasContact, long minId)
        {
            var searchBase = DefaultSelectCommand;
            var whereCommand = CreateWhereCommand(searchText, notAllowedText, sources, verbKinds, verbs, domains, maxAge, importancy, company, companyLimit, hasContact, minId);
            return string.Format("WITH q AS ({0} {1}) SELECT COUNT(*) FROM q", searchBase, whereCommand);
        }

        public static string GetQuery(this SearchingCriteriaModel searchingCriteria)
        {
            var whereCommand = CreateWhereCommand(searchingCriteria);
            var orderCommand = DefaultOrderCommand;
            var query = string.Format("SELECT * FROM [BizInfo].[dbo].[InfoSetView] {0} {1}", whereCommand, orderCommand);
            return query;
        }

        public static string CreateWhereCommand(SearchingCriteriaModel searchingCriteria)
        {
            return CreateWhereCommand(searchingCriteria.Phrase, searchingCriteria.NotAllowedPhrase, searchingCriteria.Sources, searchingCriteria.VerbKinds, searchingCriteria.Verbs, searchingCriteria.Domains, searchingCriteria.MaxAge,
                                      searchingCriteria.Important, searchingCriteria.Company, searchingCriteria.CompanyLimit, searchingCriteria.HasContact, searchingCriteria.MinId);
        }

        public static string CreateWhereCommand(string searchText, string notAllowedText, IEnumerable<int> sources, IEnumerable<int> verbKinds, IEnumerable<int> verbs, IEnumerable<int> domains, int maxAge, bool? importancy, bool? company, int companyLimit,
                                                bool? hasContact, long minId)
        {
            var searchPhrase = searchText == null ? null : searchText.Trim();
            var notAllowedPhrase = notAllowedText == null ? null : notAllowedText.Trim();
            var stringBuilder = new StringBuilder();
            if (maxAge > 0)
            {
                CreateWhereCommandOfMaxAge(stringBuilder, maxAge);
            }
            if (!SearchingCriteriaModelEx.IsAllSources(sources))
            {
                CreateWhereCommandOfTags(stringBuilder, sources, "[SourceTagId]");
            }
            if (!SearchingCriteriaModelEx.IsAllVerbKinds(verbKinds))
            {
                CreateWhereCommandOfTags(stringBuilder, verbKinds, "[VerbKindTagId]");
            }
            if (!SearchingCriteriaModelEx.IsAllVerbs(verbs))
            {
                CreateWhereCommandOfTags(stringBuilder, verbs, "[VerbTagId]");
            }
            if (!SearchingCriteriaModelEx.IsAllDomains(domains))
            {
                CreateWhereCommandOfTags(stringBuilder, domains, "[DomainTagId]");
            }
            if (importancy.HasValue)
            {
                CreateWhereCommandOfImportancy(stringBuilder, importancy.Value);
            }
            if (company.HasValue)
            {
                CreateWhereCommandOfCompany(stringBuilder, company.Value, companyLimit);
            }
            if (hasContact.HasValue)
            {
                CreateWhereCommandOfHasContact(stringBuilder, hasContact.Value);
            }
            if (minId > 0)
            {
                CreateWhereCommandOfMinId(stringBuilder, minId);
            }
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var searchSqlParameter = new EasyFts().ToFtsQuery(searchPhrase);
                stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
                stringBuilder.AppendFormat("CONTAINS (([Text], [Summary], [StructuredContent]), '{0}')", searchSqlParameter);
            }
            if (!string.IsNullOrEmpty(notAllowedPhrase))
            {
                var searchSqlParameter = new EasyFts().ToFtsQuery(notAllowedPhrase);
                stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
                stringBuilder.AppendFormat("NOT CONTAINS (([Text], [Summary], [StructuredContent]), '{0}')", searchSqlParameter);
            }
            return stringBuilder.ToString();
        }

        private static void CreateWhereCommandOfImportancy(StringBuilder stringBuilder, bool importancy)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            stringBuilder.AppendFormat(
                "{0}EXISTS (SELECT * FROM [BizInfo].[dbo].[UserAndInfoSet] WHERE [BizInfo].[dbo].[UserAndInfoSet].[InfoId] = [BizInfo].[dbo].[InfoSetView].[Id] AND [BizInfo].[dbo].[UserAndInfoSet].[UserId] = '{1}' AND [BizInfo].[dbo].[UserAndInfoSet].[IsImportant] = 1)",
                importancy ? string.Empty : "NOT ", (Guid) Membership.GetUser().ProviderUserKey);
        }

        private static void CreateWhereCommandOfMinId(StringBuilder stringBuilder, long minId)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            stringBuilder.AppendFormat("[Id] > {0}", minId);
        }

        private static void CreateWhereCommandOfHasContact(StringBuilder stringBuilder, bool hasContact)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            stringBuilder.AppendFormat("[HasContact] = {0}", hasContact ? "1" : "0");
        }

        private static void CreateWhereCommandOfCompany(StringBuilder stringBuilder, bool company, int companyLimit)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            stringBuilder.AppendFormat("[IsCompanyScore] {0} {1}", company ? "<=" : ">=", (companyLimit/100.0).ToString(CultureInfo.InvariantCulture));
        }

        private static void CreateWhereCommandOfMaxAge(StringBuilder stringBuilder, double maxAge)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            // We should to round "now" at least up to minutes, otherwise queries for count and for items will be treated as different
            var now = DateTime.Now;
            var roundedNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var minDate = roundedNow - TimeSpan.FromMinutes(maxAge);
            // stringBuilder.AppendFormat("[CreationTime] >= '{0}'", minDate.ToString("dd-MM-yyyy HH:mm:ss"));
            stringBuilder.AppendFormat("[CreationTime] >= '{0}'", minDate.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private static void CreateWhereCommandOfTags(StringBuilder stringBuilder, IEnumerable<int> tagIds, string columnName)
        {
            stringBuilder.Append(stringBuilder.Length > 0 ? " AND " : "WHERE ");
            bool containsNull;
            var sourceTagsString = GetSourceTagsString(tagIds, out containsNull);
            if (!string.IsNullOrEmpty(sourceTagsString))
            {
                if (containsNull) stringBuilder.Append("(");
                stringBuilder.AppendFormat("{1} IN ({0})", sourceTagsString, columnName);
            }
            if (containsNull)
            {
                if (!string.IsNullOrEmpty(sourceTagsString)) stringBuilder.Append(" OR ");
                stringBuilder.AppendFormat("{0} IS NULL", columnName);
                if (!string.IsNullOrEmpty(sourceTagsString)) stringBuilder.Append(")");
            }
        }

        private static string GetSourceTagsString(IEnumerable<int> sources, out bool containsNull)
        {
            var stringBuilder = new StringBuilder();
            containsNull = false;
            foreach (var source in sources)
            {
                if (source < 0)
                {
                    containsNull = true;
                    continue;
                }
                if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                stringBuilder.Append((source < 0) ? "NULL" : source.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}