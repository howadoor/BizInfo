namespace Perenis.Core.Caching.Tags
{
    /// <summary>
    /// Weak tags is a dictionary of weakly referenced keys to weak hash set of values
    /// </summary>
    public class WeakTags : Tags<WeakHashSet<object>>
    {
        #region Overrides of Tags<WeakHashSet<object>>

        protected override void _SetTag(WeakHashSet<object> collection, object tag)
        {
            collection.Add(tag);
        }

        #endregion
    }
}