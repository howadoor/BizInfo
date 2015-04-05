using System.Collections;
using System.Runtime.Remoting.Channels;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Server-side sink provider. For detail see <see cref="ProgressServerSink"/>.
    /// </summary>
    public class ProgressServerSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider _nextProvider;

        public ProgressServerSinkProvider(IDictionary properties, ICollection providerData)
        {
        }

        #region ------ Implementation of the IServerChannelSinkProvider interface -----------------

        public IServerChannelSinkProvider Next
        {
            get { return _nextProvider; }
            set { _nextProvider = value; }
        }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink next = null;
            if (_nextProvider != null)
            {
                next = _nextProvider.CreateSink(channel);
            }
            return new ProgressServerSink(next);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
        }

        #endregion
    }
}