using System.Collections.Generic;

namespace Perenis.Core.Interfaces
{
    public interface IManipulatorsCollection : IList<IManipulator>, IManipulator
    {
    }
}