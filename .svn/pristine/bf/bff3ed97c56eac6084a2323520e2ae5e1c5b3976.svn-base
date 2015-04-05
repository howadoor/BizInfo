using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizInfo.Model.Entities
{
    public static class CategoryTools
    {
        public static string GetCategoryTree()
        {
            var builder = new StringBuilder();
            using (var container = new BizInfoModelContainer())
            {
                GetCategoryTree(null, container, builder);
            }
            return builder.ToString();
        }

        private static void GetCategoryTree(Category parent, BizInfoModelContainer container, StringBuilder builder)
        {
            var categories = container.GetSubCategories(parent).ToList();
            if (categories.Count == 0) return;
            categories.Sort((c1, c2) => string.Compare(c1.Name, c2.Name, StringComparison.InvariantCultureIgnoreCase));
            foreach (var category in categories)
            {
                builder.Append("<div style=\"padding-left: 15px;\">");
                builder.Append(category.Name);
                Category category1 = category;
                builder.AppendFormat("&nbsp;<b>{0}</b>", container.InfoSet.Where(bi => bi.NativeCategory.HasValue && bi.NativeCategory.Value == category1.Id).Count());
                GetCategoryTree(category, container, builder);
                builder.Append("</div>");
            }
        }

        public static string GetAllCategories()
        {
            using (var container = new BizInfoModelContainer())
            {
                var categoryNameToCount = new Dictionary<string, int>();
                foreach(var category in container.CategorySet)
                {
                    Category category1 = category;
                    var count = container.InfoSet.Where(bi => bi.NativeCategory.HasValue && bi.NativeCategory.Value == category1.Id).Count();
                    if (!categoryNameToCount.ContainsKey(category.Name))
                    {
                        categoryNameToCount[category.Name] = count;
                    }
                    else
                    {
                        categoryNameToCount[category.Name] = categoryNameToCount[category.Name] + count;
                    }
                }
                var categories = categoryNameToCount.Keys.ToList();
                categories.Sort();
                var builder = new StringBuilder();
                var i = 1;
                foreach (var category in categories)
                {
                    builder.AppendFormat("{1}&nbsp;{0}&nbsp;{2}<br />\r\n", category, i, categoryNameToCount[category]);
                    i++;
                }
                return builder.ToString();
            }
        }

        public static List<KeyValuePair<int, List<string>>> GetCategoriesTickets()
        {
            var depthToTickets = new Dictionary<int, List<string>>();
            using (var container = new BizInfoModelContainer())
            {
                GetCategoryTickets(null, container, depthToTickets, -1);
            }
            foreach (var keyValue in depthToTickets.ToList())
            {
                var sortedList = keyValue.Value.Distinct().ToList();
                sortedList.Sort();
                depthToTickets[keyValue.Key] = sortedList;
            }
            var ticketsByDepth = depthToTickets.ToList();
            ticketsByDepth.Sort((kv1, kv2) => kv1.Key - kv2.Key);
            return ticketsByDepth;
        }

        private static void GetCategoryTickets(Category parent, BizInfoModelContainer container, Dictionary<int, List<string>> depthToTickets, int depth)
        {
            if (parent != null)
            {
                if (!depthToTickets.ContainsKey(depth))
                {
                    depthToTickets[depth] = new List<string>();
                }
                depthToTickets[depth].Add(parent.Name);
            }
            var categories = container.GetSubCategories(parent);
            foreach (var category in categories) GetCategoryTickets(category, container, depthToTickets, depth + 1);
        }

        public static IEnumerable<Category> GetCategoriesFromRoot(BizInfoModelContainer container, int? nativeCategoryId)
        {
            if (nativeCategoryId != null)
            {
                var category = container.CategorySet.Where(cat => cat.Id == nativeCategoryId.Value).SingleOrDefault();
                if (category != null)
                {
                    foreach (var parent in GetCategoriesFromRoot(container, category.Parent)) yield return parent;
                    yield return category;
                }
            }
        }
    }
}