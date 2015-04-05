using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress service interface providing passing progress from server to client.
    /// </summary>
    public interface IProgressService
    {
        /// <summary>
        /// Gets first information for the transaction on the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        IProgressInformation GetNext(Guid transactionId);

        /// <summary>
        /// Adds an information for the transaction to the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="information"></param>
        void Add(Guid transactionId, IProgressInformation information);

        /// <summary>
        /// Drops all messages for the transaction in the service queue.
        /// </summary>
        /// <param name="transactionId"></param>
        void DropTransactionId(Guid transactionId);
    }
}