using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Processing
{
    public class Tagger : ITagger
    {
        public void Tag(IBizInfo bizInfo, IBizInfoStorage storage)
        {
            string[] categories = GetCategories(bizInfo, storage);
            if (!Regex.IsMatch(categories[0], "hyperinzerce.cz|bazos.cz|annonce.cz|inzertexpres.cz|sbazar.cz|avizo.cz|bezrealitky.cz|aukro.cz")) throw new InvalidOperationException("This tagger cannot work on such informations");
            var sourceTagId = GetSourceTagId(bizInfo, storage, categories);
            var verbKindTagId = GetVerbKindTagId(bizInfo, storage, categories);
            var verbTagId = GetVerbTagId(bizInfo, storage, categories);
            var domainTagId = GetDomainTagId(bizInfo, storage, categories);
            lock (storage)
            {
                bizInfo.SourceTagId = sourceTagId;
                bizInfo.VerbKindTagId = verbKindTagId;
                bizInfo.VerbTagId = verbTagId;
                bizInfo.DomainTagId = domainTagId;
            }
        }

        private Dictionary<int, string[]> cachedCategories = new Dictionary<int, string[]>();

        private string[] GetCategories(IBizInfo bizInfo, IBizInfoStorage storage)
        {
            lock (this)
            {
                if (!bizInfo.NativeCategoryId.HasValue) return new string[]{};
                string[] categories;
                if (cachedCategories.TryGetValue(bizInfo.NativeCategoryId.Value, out categories)) return categories;
                lock (storage)
                {
                    categories = CategoryTools.GetCategoriesFromRoot(((EntityStorage)storage).modelContainer, bizInfo.NativeCategoryId).Select(cat => cat.Name).ToArray();
                }
                return categories;
            }
        }

        private int? GetVerbTagId(IBizInfo bizInfo, IBizInfoStorage storage, string[] categories)
        {
            if (categories.Where(catName => Regex.IsMatch(catName, "najm|nájem", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Nájem").Id;
            }
            if (categories.Where(catName => Regex.IsMatch(catName, "Prod|Koup", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Koupì, prodej").Id;
            }
            if (Regex.IsMatch(bizInfo.Summary, "pronaj|pronáj", RegexOptions.IgnoreCase))
            {
                return storage.GetTag("Nájem").Id;
            }
            if (Regex.IsMatch(bizInfo.Summary, "Prod|Koup", RegexOptions.IgnoreCase))
            {
                return storage.GetTag("Koupì, prodej").Id;
            }
            return null;
        }

        private int? GetSourceTagId(IBizInfo bizInfo, IBizInfoStorage storage, string[] categories)
        {
            return storage.GetTag(categories[0]).Id;
        }

        private int? GetVerbKindTagId(IBizInfo bizInfo, IBizInfoStorage storage, string[] categories)
        {
            if (categories.Where(catName => Regex.IsMatch(catName, "Nabíd", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Nabídka").Id;
            }
            if (categories.Where(catName => Regex.IsMatch(catName, "Popt", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Poptávka").Id;
            }
            return null;
        }

        private int? GetDomainTagId(IBizInfo bizInfo, IBizInfoStorage storage, string[] categories)
        {
            if (categories.Where(catName => Regex.IsMatch(catName, "Realit|Hotel|Dùm|Pozemek|Pozemky|Nemovitost|Byty|Domy", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Reality").Id;
            }
            if (categories.Where(catName => Regex.IsMatch(catName, "Auto|Moto|Auta|Vozy|Motorky|Motocykly", RegexOptions.IgnoreCase)).FirstOrDefault() != null)
            {
                return storage.GetTag("Auto, moto").Id;
            }
            return null;
        }

    }
}