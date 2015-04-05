using System;
using System.Threading;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Scouting.Management
{
    internal class ManagedScoutingAcceptor : ScoutingAcceptor
    {
        private CancellationToken cancellationToken;

        public ManagedScoutingAcceptor(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public bool Accept(string url, IBizInfoStorage storage)
        {
            return !cancellationToken.IsCancellationRequested && base.Accept(url, storage);
        }
    }
}