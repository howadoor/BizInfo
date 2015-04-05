using System.Reflection;

namespace Perenis.Core.CommandLine
{
    public interface ICommandLineProcessor
    {
        bool Parse(string[] args);

        void ExecuteOperations();
        void RegisterTypes(string nmspace, Assembly assembly);
    }
}