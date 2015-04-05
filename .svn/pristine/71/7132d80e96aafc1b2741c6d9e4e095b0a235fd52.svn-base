using System.Runtime.Serialization;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Basic implementation of <see cref="IIdProvider"/> Using <see cref="ObjectIDGenerator"/> as implementation.
    /// </summary>
    internal class IdProvider : ObjectIDGenerator, IIdProvider
    {
        #region IIdProvider Members

        public bool HasId(object @object)
        {
            bool firstTime;
            base.HasId(@object, out firstTime);
            return !firstTime;
        }

        public string GetId(object @object)
        {
            bool firstTime;
            return base.GetId(@object, out firstTime).ToString();
        }

        #endregion
    }
}