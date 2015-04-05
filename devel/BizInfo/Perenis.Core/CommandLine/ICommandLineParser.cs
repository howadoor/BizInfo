namespace Perenis.Core.CommandLine
{
    public interface ICommandLineParser
    {
        bool MoreTokens();
        bool IsOperation();
        bool IsArgument();
        string NextOperation();
        string NextArgument();
        string NextToken();
        string LastToken();
    }
}