using System.Collections.Generic;

namespace Perenis.Core.Rules
{
    public abstract class BlackAndWhiteList<TCandidate> : IBlackAndWhiteList<TCandidate>
    {
        private readonly List<ICondition<TCandidate>> _blacklist = new List<ICondition<TCandidate>>();
        private readonly List<ICondition<TCandidate>> _whitelist = new List<ICondition<TCandidate>>();

        #region IBlackAndWhiteList<TCandidate> Members

        public List<ICondition<TCandidate>> Blacklist
        {
            get { return _blacklist; }
        }

        public List<ICondition<TCandidate>> Whitelist
        {
            get { return _whitelist; }
        }

        public bool IsWhitelisted(TCandidate candidate)
        {
            return _whitelist == null || _whitelist.Count == 0 || Whitelist.Exists(whiteFilter => whiteFilter.IsMatch(candidate, true));
        }

        public bool IsBlacklisted(TCandidate candidate)
        {
            return _blacklist != null && _blacklist.Count != 0 && Blacklist.Exists(blackFilter => blackFilter.IsMatch(candidate, false));
        }

        #endregion

        #region Implementation of ICondition<TCandidate>

        public bool IsMatch(TCandidate candidate)
        {
            return IsWhitelisted(candidate) && !IsBlacklisted(candidate);
        }

        public bool IsMatch(TCandidate candidate, bool defaultMatch)
        {
            return IsMatch(candidate);
        }

        #endregion
    }
}