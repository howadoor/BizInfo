using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizInfo.Model.Entities
{
    /// <summary>
    /// Extends functionality of <see cref="SearchingCriteriaModel"/>
    /// </summary>
    public static class SearchingCriteriaModelEx
    {
        private static int?[] sourceTagIds;
        private static int?[] verbTagIds;
        private static int?[] verbKindTagIds;
        private static int?[] domainTagIds;

        public static IEnumerable<int?> SourceTagIds
        {
            get
            {
                if (sourceTagIds == null) sourceTagIds = GetTagIds("it.SourceTagId");
                return sourceTagIds;
            }
        }

        public static IEnumerable<int?> VerbsTagIds
        {
            get
            {
                if (verbTagIds == null) verbTagIds = GetTagIds("it.VerbTagId");
                return verbTagIds;
            }
        }

        public static IEnumerable<int?> VerbKindsTagIds
        {
            get
            {
                if (verbKindTagIds == null) verbKindTagIds = GetTagIds("it.VerbKindTagId");
                return verbKindTagIds;
            }
        }

        public static IEnumerable<int?> DomainsTagIds
        {
            get
            {
                if (domainTagIds == null) domainTagIds = GetTagIds("it.DomainTagId");
                return domainTagIds;
            }
        }

        private static int?[] GetTagIds(string projectionString)
        {
            using (var container = new BizInfoModelContainer())
            {
                var projection = container.InfoSet.Select(projectionString);
                var distincted = projection.Distinct().ToArray();
                return distincted.Select(dbr => dbr[0] is int ? (int?) dbr[0] : (int?) null).ToArray();
            }
        }

        public static bool IsAllSources(IEnumerable<int> sources)
        {
            return IsAll(sources, SourceTagIds);
        }

        public static bool IsAllVerbs(IEnumerable<int> verbs)
        {
            return IsAll(verbs, VerbsTagIds);
        }

        public static bool IsAllVerbKinds(IEnumerable<int> verbKinds)
        {
            return IsAll(verbKinds, VerbKindsTagIds);
        }

        public static bool IsAllDomains(IEnumerable<int> verbKinds)
        {
            return IsAll(verbKinds, VerbKindsTagIds);
        }

        private static bool IsAll(IEnumerable<int> ids, IEnumerable<int?> storedIds)
        {
            if (ids == null || ids.Count() == 0) return true;
            return storedIds.All(sti => sti.HasValue ? ids.Contains(sti.Value) : ids.Contains(-1));
        }
    }
}