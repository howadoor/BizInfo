using System;
using System.Text;

namespace BizInfo.Model.Entities
{
    public class SearchingCriteriaModel
    {
        public SearchingCriteriaModel()
        {
        }

        protected SearchingCriteriaModel(SearchingCriteriaModel searchingCriteria)
        {
            Phrase = searchingCriteria.Phrase;
            NotAllowedPhrase = searchingCriteria.NotAllowedPhrase;
            Sources = searchingCriteria.Sources;
            VerbKinds = searchingCriteria.VerbKinds;
            Verbs = searchingCriteria.Verbs;
            Domains = searchingCriteria.Domains;
            Important = searchingCriteria.Important;
            Company = searchingCriteria.Company;
            CompanyLimit = searchingCriteria.CompanyLimit;
            MaxAge = searchingCriteria.MaxAge;
            HasContact = searchingCriteria.HasContact;
            KindOfView = searchingCriteria.KindOfView;
        }

        public string Phrase { get; set; }

        /// <summary>
        /// Defines which words or fragments are NOT allowed in the searched informations
        /// </summary>
        public string NotAllowedPhrase { get; set; }

        public int[] Sources { get; set; }
        public int[] VerbKinds { get; set; }
        public int[] Verbs { get; set; }
        public int[] Domains { get; set; }
        public bool? Important { get; set; }
        public bool? Company { get; set; }
        public bool? HasContact { get; set; }
        public int CompanyLimit { get; set; }
        public int MaxAge { get; set; }
        public int KindOfView { get; set; }
        public long MinId { get; set; }

        public static SearchingCriteriaModel CreateDefault()
        {
            return new SearchingCriteriaModel
                       {
                           MaxAge = 7*24*60,
                           CompanyLimit = 50,
                           HasContact = true,
                           KindOfView = 1
                       };
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder);
            return builder.ToString();
        }

        protected virtual void ToString(StringBuilder builder)
        {
            if (!string.IsNullOrEmpty(Phrase)) builder.AppendFormat("Phrase \"{0}\" ", Phrase);
            if (!string.IsNullOrEmpty(NotAllowedPhrase)) builder.AppendFormat("NotAllowedPhrase \"{0}\" ", NotAllowedPhrase);
            if (!SearchingCriteriaModelEx.IsAllSources(Sources)) AddToString("Sources", Sources, builder);
            if (!SearchingCriteriaModelEx.IsAllSources(VerbKinds)) AddToString("VerbKinds", VerbKinds, builder);
            if (!SearchingCriteriaModelEx.IsAllSources(Verbs)) AddToString("Verbs", Verbs, builder);
            if (!SearchingCriteriaModelEx.IsAllSources(Domains)) AddToString("Domains", Domains, builder);
            if (Important.HasValue) builder.AppendFormat("Important {0} ", Important.Value);
            if (Company.HasValue) builder.AppendFormat("Company {0} {1} ", Company.Value, CompanyLimit);
            if (HasContact.HasValue) builder.AppendFormat("HasContact {0} ", HasContact.Value);
            if (MaxAge > 0) builder.AppendFormat("MaxAge {0} {1} ", MaxAge, TimeSpan.FromMinutes(MaxAge));
            if (MinId > 0) builder.AppendFormat("MinId {0} ", MinId);
            builder.AppendFormat("View {0}", KindOfView);
        }

        protected void AddToString(string name, int[] ids, StringBuilder builder)
        {
            if (ids == null || ids.Length == 0) return;
            builder.AppendFormat("{0} [", name);
            AddToString(ids, builder);
            builder.Append("] ");
        }

        protected void AddToString(int[] ids, StringBuilder builder)
        {
            foreach (var id in ids)
            {
                builder.AppendFormat("{0}, ", id);
            }
        }
    }
}