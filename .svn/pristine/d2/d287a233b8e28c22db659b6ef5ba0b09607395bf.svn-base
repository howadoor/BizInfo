using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Storage
{
    public class EntityStorage : IWebSourceStorage, IBizInfoStorage
    {
        protected readonly Dictionary<string, int> cachedCategories = new Dictionary<string, int>();
        private UrlDownloader loader;
        public BizInfoModelContainer modelContainer;
        private Dictionary<string, Tag> cachedTags = new Dictionary<string, Tag>();

        #region Constructors
        
        public EntityStorage()
        {
            modelContainer = new BizInfoModelContainer();
        }

        public EntityStorage(string connectionString)
        {
            modelContainer = new BizInfoModelContainer(connectionString);
        }

        public EntityStorage(EntityConnection connection)
        {
            modelContainer = new BizInfoModelContainer(connection);
        }

        #endregion

        #region IBizInfoStorage Members

        public IEnumerable<IBizInfo> Infos
        {
            get { return modelContainer.InfoSet; }
        }

        public IBizInfo CreateInfo(IWebSource webSource, string summary, string text, DateTime creationTime)
        {
            lock (this)
            {
                var info = modelContainer.InfoSet.CreateObject();
                modelContainer.InfoSet.AddObject(info);
                info.Summary = summary;
                info.Text = text;
                info.WebSourceId = webSource.Id;
                info.CreationTime = creationTime;
                return info;
            }
        }

        public virtual int GetCategory(string name, int? parentCategory)
        {
            lock (this)
            {
                var categoryKey = parentCategory.HasValue ? string.Format("{0}^{1}", name, parentCategory) : name;
                int categoryId;
                if (cachedCategories.TryGetValue(categoryKey, out categoryId)) return categoryId;
                var categories = modelContainer.CategorySet.Where(cat => ((parentCategory == null && cat.Parent == null) || (parentCategory == cat.Parent)) && cat.Name.Equals(name));
                Category category;
                try
                {
                    category = categories.SingleOrDefault();
                }
                catch (Exception exception)
                {
                    this.LogInfo(string.Format("Non-unique category {0}", categoryKey));
                    this.LogException(exception);
                    throw;
                }
                if (category == null)
                {
                    category = modelContainer.CategorySet.CreateObject();
                    modelContainer.CategorySet.AddObject(category);
                    category.Name = name;
                    category.Parent = parentCategory;
                    modelContainer.SaveChanges();
                }
                else
                {
                    if (category.Name != name || category.Parent != parentCategory) throw new InvalidOperationException("Wrong category found");
                }
                cachedCategories[categoryKey] = category.Id;
                return category.Id;
            }
        }

        public IUrlDownloader Loader
        {
            get { return loader ?? (loader = new UrlDownloader(modelContainer)); }
        }

        public bool TryAddAssScouted(string url)
        {
            lock (this)
            {
                var webSourceExists = modelContainer.WebSourceSet.Any(ws => ws.Url == url);
                if (webSourceExists) return false;
                var webSource = modelContainer.WebSourceSet.CreateObject();
                modelContainer.WebSourceSet.AddObject(webSource);
                webSource.Url = url;
                webSource.Scouted = DateTime.Now;
                modelContainer.SaveChanges();
                modelContainer.WebSourceSet.Detach(webSource);
                return true;
            }
        }

        public Tag GetTag(string tagName)
        {
            lock (this)
            {
                Tag tag;
                if (cachedTags.TryGetValue(tagName, out tag)) return tag;
                tag = modelContainer.TagSet.Where(t => t.Name == tagName).SingleOrDefault();
                if (tag == null)
                {
                    tag = modelContainer.TagSet.CreateObject();
                    modelContainer.TagSet.AddObject(tag);
                    tag.Name = tagName;
                    modelContainer.SaveChanges();
                }
                cachedTags[tagName] = tag;
                return tag;
            }
        }

        public void Delete(IWebSource webSource)
        {
            lock (this)
            {
                loader.DeleteFromCache(webSource.Url);
                modelContainer.WebSourceSet.DeleteObject((WebSource) webSource);
            }
        }

        public void ScheduleReload(IBizInfo info, DateTime reloadDate)
        {
            modelContainer.ScheduleReload(((Info) info).Id, reloadDate);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            modelContainer.Dispose();
            modelContainer = null;
        }

        #endregion

        #region IWebSourceStorage Members

        public IEnumerable<IWebSource> Sources
        {
            get { return modelContainer.WebSourceSet; }
        }

        #endregion
    }
}