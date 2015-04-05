using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Common
{
    public class PagingModel : SearchingCriteriaModel
    {
        public PagingModel()
        {
        }

        public PagingModel(SearchingCriteriaModel searchingCriteria) : base(searchingCriteria)
        {
        }

        [ScriptIgnore]
        [IgnoreDataMember]
        public int FromIndex
        {
            get { return Math.Min(TotalCount, CurrentPage*ItemsPerPage); }
        }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        [IgnoreDataMember]
        public int TotalCount { get; set; }

        [ScriptIgnore]
        [IgnoreDataMember]
        public IEnumerable<int> NavigationPageIndices
        {
            get
            {
                var maxPage = Math.Min(Math.Ceiling((double) TotalCount/ItemsPerPage), CurrentPage + 5);
                for (var i = Math.Max(0, CurrentPage - 5); i < maxPage; i++) yield return i;
            }
        }

        [IgnoreDataMember]
        public int StartIndex
        {
            get { return Math.Min(TotalCount, CurrentPage*ItemsPerPage); }
        }

        [IgnoreDataMember]
        public int EndIndex
        {
            get { return Math.Min(TotalCount, (CurrentPage + 1)*ItemsPerPage); }
        }

        protected override void ToString(StringBuilder builder)
        {
            base.ToString(builder);
            builder.AppendFormat(" CurrentPage {0}", CurrentPage);
            builder.AppendFormat(" ItemsPerPage {0}", ItemsPerPage);
            builder.AppendFormat(" TotalCount {0}", TotalCount);
        }
    }
}