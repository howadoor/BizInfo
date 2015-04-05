namespace Perenis.Core.Interfaces
{
    public abstract class Manipulator<TManipulated> : IManipulator<TManipulated>
    {
        #region IManipulator<TManipulated> Members

        public void Attach(object manipulated)
        {
            Attach((TManipulated) manipulated);
        }

        public void Detach(object manipulated)
        {
            Detach((TManipulated) manipulated);
        }

        public abstract void Attach(TManipulated manipulated);
        public abstract void Detach(TManipulated manipulated);

        #endregion
    }
}