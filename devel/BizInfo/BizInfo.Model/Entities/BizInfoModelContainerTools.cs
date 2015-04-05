using System;
using System.Linq;
using System.Text;

namespace BizInfo.Model.Entities
{
    public static class BizInfoModelContainerTools
    {
        public static WebSource GetWebSourceOf(this BizInfoModelContainer container, Info bizInfo)
        {
            if (!bizInfo.WebSourceId.HasValue) return null;
            var webSourceId = bizInfo.WebSourceId.Value;
            return container.WebSourceSet.Where(ws => ws.Id == webSourceId).SingleOrDefault();
        }

        public static string GetCategoryFullName(this BizInfoModelContainer container, Category category, string elementsDelimiter)
        {
            if (category == null) return null;
            if (category.Parent != null) return GetCategoryFullName(container, GetCategory(container, category.Parent.Value), elementsDelimiter) + elementsDelimiter + category.Name;
            return category.Name;
        }

        public static Category GetCategory(this BizInfoModelContainer container, int categoryId)
        {
            return container.CategorySet.Where(cat => cat.Id == categoryId).FirstOrDefault();
        }

        public static IQueryable<Category> GetSubCategories(this BizInfoModelContainer container, Category parentCategory)
        {
            return parentCategory == null ? container.CategorySet.Where(cat => !cat.Parent.HasValue) : container.CategorySet.Where(cat => cat.Parent.HasValue && (cat.Parent.Value == parentCategory.Id));
        }

        public static ScheduledReload ScheduleReload(this BizInfoModelContainer container, long infoId, DateTime reloadTime)
        {
            var scheduledReload = container.GetOrCreateScheduledReload(infoId);
            scheduledReload.ScheduledTime = reloadTime;
            return scheduledReload;
        }

        private static ScheduledReload GetOrCreateScheduledReload(this BizInfoModelContainer container, long infoId)
        {
            lock (container)
            {
                var scheduledReload = container.ScheduledReloadSet.Where(sr => sr.InfoId == infoId).SingleOrDefault();
                if (scheduledReload == null)
                {
                    scheduledReload = container.ScheduledReloadSet.CreateObject();
                    scheduledReload.InfoId = infoId;
                    container.ScheduledReloadSet.AddObject(scheduledReload);
                }
                return scheduledReload;
            }
        }
    }
}