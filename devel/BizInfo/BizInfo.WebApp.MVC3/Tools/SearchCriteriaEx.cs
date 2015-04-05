using System;
using System.Linq;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Models.Common;
using Perenis.Core.Serialization;

namespace BizInfo.WebApp.MVC3.Tools
{
    public static class SearchCriteriaEx
    {
        public static string GetDisplayString(this SearchCriteria searchCriteria)
        {
            var criteria = GetCriteriaModel(searchCriteria);
            return criteria.ToString();
        }

        public static SearchingCriteriaModel GetCriteriaModel(this SearchCriteria searchCriteria)
        {
            // TODO: Realy ugly workarround
            var criteria = searchCriteria.Criteria;
            if (criteria.StartsWith("<PagingModel"))
            {
                return XmlSerialization.FromXmlStringByDataContractSerializer<PagingModel>(criteria);
            }
            if (criteria.StartsWith("<SearchingCriteriaModel"))
            {
                return XmlSerialization.FromXmlString<SearchingCriteriaModel>(criteria);
            }
            throw new NotImplementedException("Unknown type of search criteria");
        }

        public static SearchCriteria GetSearchCriteria(Guid searchingCriteriaHash)
        {
            using (var repository = new BizInfoModelContainer())
            {
                return repository.SearchCriteriaSet.SingleOrDefault(sc => sc.Hash == searchingCriteriaHash);
            }
        }

        public static string GetSearchUrl(this SearchCriteria searchCriteria)
        {
            return string.Format("~/Base/SearchingCriteria?id={0}", searchCriteria.Id);
        }

        public static SearchCriteria GetSearchCriteria(long searchingCriteriaId)
        {
            using (var repository = new BizInfoModelContainer())
            {
                return repository.SearchCriteriaSet.SingleOrDefault(sc => sc.Id == searchingCriteriaId);
            }
        }
    }
}