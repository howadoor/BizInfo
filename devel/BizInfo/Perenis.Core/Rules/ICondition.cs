namespace Perenis.Core.Rules
{
    public interface ICondition<TCandidate>
    {
        bool IsMatch(TCandidate candidate);
        bool IsMatch(TCandidate candidate, bool defaultMatch);
    }
}