using System.Collections.Generic;

namespace Perenis.Core.Interfaces
{
    /// <summary>
    /// Resolve name to objects(s)
    /// </summary>
    public interface INameResolver
    {
        IEnumerable<TObject> Resolve<TObject>(string name, object context = null);
    }
}