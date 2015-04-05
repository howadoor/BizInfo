namespace Perenis.Core.Interfaces
{
    public interface IHosted
    {
        object Host { get; }
    }

    public interface IHosted<out THost>
    {
        THost Host { get;}
    }
}

