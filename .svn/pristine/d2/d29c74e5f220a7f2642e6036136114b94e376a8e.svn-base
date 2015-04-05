using System.Collections.Generic;
using System.Linq;
using Perenis.Core.Decoupling.Multimethods;

namespace Perenis.Core.General
{
    /// <summary>
    /// Universal implementation of IAware, using multimethods (multifunction).
    /// </summary>
    public class Aware : IAware
    {
        private MultiMethod.Function<object> _aware;

        public IEnumerable<object> Services
        {
            set { _aware = ServiceDispatcher.Function<object, object>(GetAwared<object>, value.ToArray()); }
        }

        #region IAware Members

        public TRequested GetAwared<TRequested>(object @object)
        {
            return (TRequested) _aware(typeof (TRequested), @object).ReturnValue;
        }

        #endregion
    }
}