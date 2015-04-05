using System.Collections;
using System.Runtime.Remoting.Channels;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Client-side sink provider. For detail see <see cref="ProgressClientSink"/>.
    /// </summary>
    public class ProgressClientSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider next;

        public ProgressClientSinkProvider()
        {
        }

        public ProgressClientSinkProvider(IDictionary properties, ICollection providerData)
        {
        }

        #region IClientChannelSinkProvider Members

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            string forcedUrl = RemotingServerUrlWorkarround.ForcedServerUrl;
            url = string.IsNullOrEmpty(forcedUrl) ? url : forcedUrl;

            IClientChannelSink nextSink = null;

            // checking for additional sink providers
            if (next != null)
            {
                nextSink = next.CreateSink(channel, url, remoteChannelData);
            }
            // returning first entry of a sink chain
            return new ProgressClientSink(nextSink);
        }

        public IClientChannelSinkProvider Next
        {
            get { return next; }
            set { next = value; }
        }

        #endregion
    }
}