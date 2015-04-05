using System;
using System.Collections.Generic;
using Perenis.Core.Log;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress service providing passing progress from server to client.
    /// </summary>
    public class ProgressService : MarshalByRefObject, IProgressService
    {
        private static readonly Dictionary<Guid, List<IProgressInformation>> queue = new Dictionary<Guid, List<IProgressInformation>>();
        private static readonly object queueLock = new object();

        #region IProgressService Members

        /// <summary>
        /// Adds a message for the transaction to the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="information"></param>
        public void Add(Guid transactionId, IProgressInformation information)
        {
            lock (queueLock)
            {
                if (!queue.ContainsKey(transactionId))
                {
                    queue[transactionId] = new List<IProgressInformation>();
                }
                queue[transactionId].Add(information);
            }
        }

        /// <summary>
        /// Drops all messages for the transaction in the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        public void DropTransactionId(Guid transactionId)
        {
            lock (queueLock)
            {
                if (queue.ContainsKey(transactionId))
                {
                    if (queue[transactionId].Count > 0)
                    {
                        this.LogWarn("Dropping transaction with not read all messages.");
                    }
                    queue.Remove(transactionId);
                }
            }
        }

        /// <summary>
        /// Gets first information for the transaction on the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public IProgressInformation GetNext(Guid transactionId)
        {
            IProgressInformation res = null;
            lock (queueLock)
            {
                if (queue.ContainsKey(transactionId) && queue[transactionId].Count > 0)
                {
                    res = queue[transactionId][0];
                    queue[transactionId].RemoveAt(0);
                }
            }
            return res;
        }

        #endregion
    }
}