namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Well-known singleton pattern implementation.
    /// </summary>
    /// <remarks>
    /// The actual instance type is determined by the only type parameter. The singleton type must implement a default constructor.
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var singleton = Singleton<Something>.Instance;
    /// ]]></code>
    /// </example>
    /// </remarks>
    /// <typeparam name="TInstance"></typeparam>
    public class Singleton<TInstance> where TInstance : class, new()
    {
        private static readonly TInstance instance = new TInstance();

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static TInstance Instance
        {
            get { return instance; }
        }
    }
}