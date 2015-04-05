using System.Collections.Generic;

namespace Perenis.Core.Rules
{
    public interface IBlackAndWhiteList<TCandidate> : ICondition<TCandidate>
    {
        List<ICondition<TCandidate>> Blacklist { get; }
        List<ICondition<TCandidate>> Whitelist { get; }
        bool IsWhitelisted(TCandidate candidate);
        bool IsBlacklisted(TCandidate candidate);
    }
}