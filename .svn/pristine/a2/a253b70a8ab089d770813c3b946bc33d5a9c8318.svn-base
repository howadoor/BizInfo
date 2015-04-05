namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Assigns and caches objects ids for purpose of serialization. 
    /// </summary>
    public interface IIdProvider
    {
        /// <summary>
        /// Checks if the bject has already assigned id. Such object should not be stored again as a whole value but only as a reference to previously stored content.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        bool HasId(object @object);

        /// <summary>
        /// Returns id of the object. If object has an id already assigned, it returns the cached one. Otherwise assigns
        /// new id, stores it to the id cache and returns it.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        string GetId(object @object);
    }
}