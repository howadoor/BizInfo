using System.Collections.Generic;
using System.Linq;
using Perenis.Core.Serialization;

namespace BizInfo.Model.Entities.Extensions
{
    /// <summary>
    /// Collects extension methods of <see cref="Tenant"/>
    /// </summary>
    public static class TenantEx
    {
        /// <summary>
        /// Determines if any watches belonged to <see cref="tenant"/> is active
        /// </summary>
        /// <param name="tenant">Owner tenant of the watches</param>
        /// <returns><c>true</c> if any active watch exists, <c>false</c> otherwise</returns>
        public static bool HasAnyActiveWatches(this Tenant tenant)
        {
            return tenant.WatchSet.Any(watch => watch.IsActive);
        }

        /// <summary>
        /// Returns all active watches belonged to <see cref="tenant"/>
        /// </summary>
        /// <param name="tenant">Owner tenant of the watches</param>
        /// <returns>Collection of active watches belonged to <see cref="tenant"/></returns>
        public static IEnumerable<Watch> GetActiveWatches(this Tenant tenant)
        {
            return tenant.WatchSet.Where(watch => watch.IsActive);
        }

        /// <summary>
        /// Returns collection of actively watched search criteria
        /// </summary>
        /// <param name="tenant">Owner tenant</param>
        /// <returns>Collection of actively watched search criteria</returns>
        public static IEnumerable<SearchingCriteriaModel> GetActiveWatchedCriteria(this Tenant tenant)
        {
            return tenant.GetActiveWatches().Select(watch => XmlSerialization.FromXmlString<SearchingCriteriaModel>(watch.SearchCriteria.Criteria));
        }
    }
}