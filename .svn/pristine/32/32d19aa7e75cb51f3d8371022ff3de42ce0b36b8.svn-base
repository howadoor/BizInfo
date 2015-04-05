using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Client-side sink providing progress transwer from server to client.
    /// </summary>
    public class ProgressClientSink : BaseChannelSinkWithProperties, IClientChannelSink
    {
        private readonly IClientChannelSink _nextSink;

        public ProgressClientSink(IClientChannelSink next)
        {
            _nextSink = next;
        }

        #region IClientChannelSink Members

        public IClientChannelSink NextChannelSink
        {
            get { return _nextSink; }
        }

        public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            // note: not intended to be used for asynchronous call - designed only for synchronous call (via ProcessMessage)

            sinkStack.Push(this, null);
            _nextSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }

        public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
        {
            // note: not intended to be used for asynchronous call - designed only for synchronous call (via ProcessMessage)

            sinkStack.AsyncProcessResponse(headers, stream);
        }

        public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
        {
            return _nextSink.GetRequestStream(msg, headers);
        }

        public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            if (Progress.TopProgress != null && msg.Properties.Contains("__TypeName") && !msg.Properties["__TypeName"].Equals(typeof (IProgressService).AssemblyQualifiedName) &&
                !msg.Properties["__TypeName"].Equals(typeof (IProgressMessage).AssemblyQualifiedName))
            {
                requestHeaders["Progress_TaskName"] = Progress.TopProgress.TaskName;
                requestHeaders["Progress_TransactionId"] = Progress.TopProgress.TransactionId;
                requestHeaders["Progress_ScaleMin"] = Progress.TopProgress.ScaleMin;
                requestHeaders["Progress_ScaleMax"] = Progress.TopProgress.ScaleMax;
                requestHeaders["Progress_Progress"] = Progress.TopProgress.Progress;
                using (Poller.GetPoller(msg.Properties["__TypeName"] as string, msg.Properties["__MethodName"] as string))
                {
                    _nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
                }
            }
            else
            {
                _nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
            }
        }

        #endregion
    }
}