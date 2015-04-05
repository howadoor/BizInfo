using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Server-side sink providing progress transwer from server to client.
    /// </summary>
    public class ProgressServerSink : BaseChannelSinkWithProperties, IServerChannelSink
    {
        private readonly IServerChannelSink _nextSink;

        public ProgressServerSink(IServerChannelSink next)
        {
            _nextSink = next;
        }

        #region ------ Implementation of the IServerChannelSink interface -------------------------

        public IServerChannelSink NextChannelSink
        {
            get { return _nextSink; }
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            // note: not intended to be used for asynchronous call - designed only for synchronous call (via ProcessMessage)

            // forwarding to the stack for further processing
            sinkStack.AsyncProcessResponse(msg, headers, stream);
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
        {
            return null;
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            if (_nextSink != null)
            {
                bool wasSet = false;
                if (Progress.TopProgress == null && GuidEx.IsGuid(requestHeaders["Progress_TransactionId"] as string))
                {
                    var taskName = requestHeaders["Progress_TaskName"] as string;
                    Guid transactionId = GuidEx.TryParse(requestHeaders["Progress_TransactionId"] as string).Value;
                    int scaleMin;
                    int.TryParse(requestHeaders["Progress_ScaleMin"] as string, out scaleMin);
                    int scaleMax;
                    int.TryParse(requestHeaders["Progress_ScaleMax"] as string, out scaleMax);
                    int progress;
                    int.TryParse(requestHeaders["Progress_Progress"] as string, out progress);
                    Progress.TopProgress = new ProxyProgressTransaction(taskName, transactionId, scaleMin, scaleMax, progress);
                    wasSet = true;
                }

                // pushing onto stack and forwarding the call
                sinkStack.Push(this, null);
                ServerProcessing srvProc = _nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);

                if (wasSet)
                {
                    Progress.TopProgress = null;
                }

                // returning status information
                return srvProc;
            }
            else
            {
                responseMsg = null;
                responseHeaders = null;
                responseStream = null;
                return new ServerProcessing();
            }
        }

        #endregion
    }
}