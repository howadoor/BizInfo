namespace Perenis.Core.Interfaces
{
    /// <summary>
    /// Interface of general manipulator. Manipulator attaches to manipulated object, typically
    /// adds some manipulation routines to its events then, when internal conditions reaches, makes
    /// a manipulation.
    /// </summary>    
    public interface IManipulator : IAttachable
    {
    }

    /// <summary>
    /// Interface of general manipulator. Manipulator attaches to manipulated object, typically
    /// adds some manipulation routines to its events then, when internal conditions reaches, makes
    /// a manipulation. Typed version of <see cref="IManipulator"/>.
    /// </summary>
    public interface IManipulator<TManipulated> : IManipulator, IAttachable<TManipulated>
    {
    }
}