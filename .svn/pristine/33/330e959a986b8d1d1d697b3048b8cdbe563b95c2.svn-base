using System.Runtime.Remoting.Messaging;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Workarround about .NET Remoting problem which occurs when server ahs multiple adapters (and other similar circumstances). 
    /// </summary>
    public static class RemotingServerUrlWorkarround
    {
        private const string ForcedServerUrlId = "ForcedServerUrl";

        /// <summary>
        /// Forced url of the server. It should be used instead URL sent by server, because it is not generally accessible from client
        /// </summary>
        public static string ForcedServerUrl
        {
            get { return CallContext.GetData(ForcedServerUrlId) as string; }
            set { CallContext.SetData(ForcedServerUrlId, value); }
        }
    }
}