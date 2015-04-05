using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
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

        public static SelectList GetMaxAgeSelectList(this SearchingCriteriaModel model)
        {
            var items = model.GetMaxAgeSelectListItems().ToArray();
            return new SelectList(items, "Value", "Text", items.Min(item => Math.Abs(item.Value - model.MaxAge)));
        }

        public static IEnumerable<dynamic> GetMaxAgeSelectListItems(this SearchingCriteriaModel model)
        {
            yield return new {Selected = false, Text = "1 hodinu", Value = 1*60};
            yield return new {Selected = false, Text = "2 hodiny", Value = 2*60};
            yield return new {Selected = false, Text = "3 hodiny", Value = 3*60};
            yield return new {Selected = false, Text = "6 hodin", Value = 6*60};
            yield return new {Selected = false, Text = "12 hodin", Value = 12*60};
            yield return new {Selected = false, Text = "1 den", Value = 1*24*60};
            yield return new {Selected = false, Text = "2 dny", Value = 2*24*60};
            yield return new {Selected = false, Text = "3 dny", Value = 3*24*60};
            yield return new {Selected = false, Text = "1 týden", Value = 7*24*60};
            yield return new {Selected = false, Text = "2 týdny", Value = 14*24*60};
            yield return new {Selected = true, Text = "1 měsíc", Value = 30*24*60};
            yield return new {Selected = false, Text = "2 měsíce", Value = 60*24*60};
            yield return new {Selected = false, Text = "6 měsíců", Value = 180*24*60};
            yield return new {Selected = false, Text = "1 rok", Value = 365*24*60};
            yield return new {Selected = false, Text = "Bez omezení", Value = 0};
        }

        public static SelectList GetImportancySelectList(this SearchingCriteriaModel model)
        {
            var items = model.GetImportancySelectListItems().ToArray();
            var list = new SelectList(items, "Value", "Text", items.Where(i => ((bool?) i.Value) == model.Important).Single());
            return list;
        }

        public static IEnumerable<dynamic> GetImportancySelectListItems(this SearchingCriteriaModel model)
        {
            yield return new {Selected = false, Text = "Všechny", Value = (bool?) null};
            yield return new {Selected = false, Text = "Důležité", Value = (bool?) true};
            yield return new {Selected = false, Text = "Nedůležité", Value = (bool?) false};
        }

        public static SelectList GetKindOfViewSelectList(this SearchingCriteriaModel model)
        {
            var items = model.GetKindOfViewSelectListItems().ToArray();
            var list = new SelectList(items, "Value", "Text", items.Where(i => ((int) i.Value) == model.KindOfView).Single());
            return list;
        }

        public static IEnumerable<dynamic> GetKindOfViewSelectListItems(this SearchingCriteriaModel model)
        {
            yield return new {Selected = false, Text = "Podrobné", Value = 0};
            yield return new {Selected = false, Text = "Stručné", Value = 1};
        }

        public static SelectList GetHasContactSelectList(this SearchingCriteriaModel model)
        {
            var items = model.GetHasContactSelectListItems().ToArray();
            var list = new SelectList(items, "Value", "Text", items.Where(i => ((bool?) i.Value) == model.HasContact).Single());
            return list;
        }

        public static IEnumerable<dynamic> GetHasContactSelectListItems(this SearchingCriteriaModel model)
        {
            yield return new {Selected = false, Text = "Všechny", Value = (bool?) null};
            yield return new {Selected = false, Text = "Pouze s kontaktem", Value = (bool?) true};
            yield return new {Selected = false, Text = "Pouze bez kontaktu", Value = (bool?) false};
        }

        public static SelectList GetCompanySelectList(this SearchingCriteriaModel model)
        {
            var items = model.GetCompanySelectListItems().ToArray();
            var list = new SelectList(items, "Value", "Text", items.Where(i => ((bool?) i.Value) == model.Company).Single());
            return list;
        }

        public static IEnumerable<dynamic> GetCompanySelectListItems(this SearchingCriteriaModel model)
        {
            yield return new {Selected = false, Text = "Všechny", Value = (bool?) null};
            yield return new {Selected = false, Text = "Od osob", Value = (bool?) true};
            yield return new {Selected = false, Text = "Od firem", Value = (bool?) false};
        }

        public static string GetSourcesAsString(this SearchingCriteriaModel model)
        {
            return GetIdsAsString(model.Sources, SourceTagIds);
        }

        public static string GetVerbsAsString(this SearchingCriteriaModel model)
        {
            return GetIdsAsString(model.Verbs, VerbsTagIds);
        }

        public static string GetVerbKindsAsString(this SearchingCriteriaModel model)
        {
            return GetIdsAsString(model.VerbKinds, VerbKindsTagIds);
        }

        public static string GetDomainsAsString(this SearchingCriteriaModel model)
        {
            return GetIdsAsString(model.Domains, DomainsTagIds);
        }

        private static string GetIdsAsString(int[] ids, IEnumerable<int?> storedIds)
        {
            if (IsAll(ids, storedIds)) return null;
            var stringBuilder = new StringBuilder();
            foreach (var source in ids)
            {
                if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                stringBuilder.Append(source.ToString());
            }
            return stringBuilder.ToString();
        }

        public static IEnumerable<SelectListItem> GetSourcesSelectListItems(this SearchingCriteriaModel model)
        {
            return GetSelectListItems(SourceTagIds, model.Sources);
        }

        public static IEnumerable<SelectListItem> GetVerbKindsSelectListItems(this SearchingCriteriaModel model)
        {
            return GetSelectListItems(VerbKindsTagIds, model.VerbKinds);
        }

        public static IEnumerable<SelectListItem> GetVerbsSelectListItems(this SearchingCriteriaModel model)
        {
            return GetSelectListItems(VerbsTagIds, model.Verbs);
        }

        public static IEnumerable<SelectListItem> GetDomainsSelectListItems(this SearchingCriteriaModel model)
        {
            return GetSelectListItems(DomainsTagIds, model.Domains);
        }

        private static List<SelectListItem> GetSelectListItems(IEnumerable<int?> tagIds, int[] searchModelTagIds)
        {
            var all = searchModelTagIds == null;
            using (var container = new BizInfoModelContainer())
            {
                var listItems = tagIds.Select(tagId =>
                                                  {
                                                      if (!tagId.HasValue) return new SelectListItem {Selected = true, Text = "Ostatní, neurčeno", Value = "-1"};
                                                      var tag = container.TagSet.Where(t => t.Id == tagId.Value).Single();
                                                      return new SelectListItem {Selected = all || searchModelTagIds.Contains(tag.Id), Text = tag.Name, Value = tag.Id.ToString()};
                                                  }).ToList();
                listItems.Sort((s1, s2) => string.Compare(s1.Text, s2.Text, StringComparison.CurrentCultureIgnoreCase));
                return listItems;
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