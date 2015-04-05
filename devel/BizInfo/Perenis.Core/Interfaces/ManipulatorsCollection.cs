using System.Collections.Generic;

namespace Perenis.Core.Interfaces
{
    internal class ManipulatorsCollection : List<IManipulator>, IManipulatorsCollection
    {
        public ManipulatorsCollection()
        {
        }

        public ManipulatorsCollection(params IManipulator[] manipulators)
        {
            AddRange(manipulators);
        }

        #region IManipulatorsCollection Members

        public void Attach(object manipulated)
        {
            foreach (var manipulator in this) manipulator.Attach(manipulated);
        }

        public void Detach(object manipulated)
        {
            foreach (var manipulator in this) manipulator.Detach(manipulated);
        }

        #endregion
    }
}