using System.Collections.Generic;

namespace Perenis.Core.Caching.Tags
{
    /// <summary>
    /// String tags is a dictionary of weakly referenced keys to strong hashset of values
    /// </summary>
    public class StrongTags : Tags<HashSet<object>>
    {
        #region Overrides of Tags<HashSet<object>>

        protected override void _SetTag(HashSet<object> collection, object tag)
        {
            collection.Add(tag);
        }

        #endregion
    }
}