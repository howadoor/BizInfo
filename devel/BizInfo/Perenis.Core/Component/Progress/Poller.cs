using System;
using System.Diagnostics;
using System.Threading;
using Perenis.Core.Log;
using Perenis.Core.Pattern;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Helper class used for polling of progress during synchronous invoke via .NET remoting.
    /// </summary>
    public class Poller : Disposable
    {
        private const int pollerInterval = 500;
        private readonly string methodName;
        private readonly IProgressService progressService;
        private readonly IProgressTransaction progressTransaction;
        private readonly Thread thread;
        private readonly Guid transactionId;
        private readonly string typeName;

        /// <summary>
        /// Creates new instance of this class.
        /// </summary>
        /// <param name="typeName">Type of the called object (obtained from message)</param>
        /// <param name="methodName"></param>
        private Poller(string typeName, string methodName)
        {
            progressService = Progress.GetService(Progress.ProgressServiceUri);
            progressTransaction = Progress.TopProgress;
            transactionId = progressTransaction != null ? progressTransaction.TransactionId : Guid.Empty;
            this.typeName = typeName;
            this.methodName = methodName;
            thread = new Thread(Process);
            Debug.WriteLine(string.Format("Poller initialized (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
        }

        /// <summary>
        /// Creator of instance of this class.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static Poller GetPoller(string typeName, string methodName)
        {
            var p = new Poller(typeName, methodName);
            p.Start();
            return p;
        }

        /// <summary>
        /// Starts polling.
        /// </summary>
        private void Start()
        {
            Debug.WriteLine(string.Format("Poller starting (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
            thread.Start();
        }

        /// <summary>
        /// Finishes polling.
        /// </summary>
        private void Finish()
        {
            if (thread.IsAlive)
            {
                Debug.WriteLine(string.Format("Poller finishing (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
                thread.Interrupt();
                thread.Join();
            }
        }

        /// <summary>
        /// Worker thread method.
        /// </summary>
        private void Process()
        {
            Debug.WriteLine(string.Format("Poller started (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
            while (true)
            {
                try
                {
                    IProgressInformation information;
                    while ((information = progressService.GetNext(transactionId)) != null)
                    {
                        var advance = information as IProgressAdvance;
                        if (advance != null)
                        {
                            Debug.WriteLine(string.Format("Poller got an advance (TransactionId: {0}; Method: {1}; Type: {2}; Advance: {3}).", transactionId, methodName, typeName, advance.Advance));
                            if (progressTransaction != null)
                            {
                                progressTransaction.Advance(advance.Advance);
                            }
                            else
                            {
                                Debug.WriteLine(string.Format("Poller lost an advance (TransactionId: {0}; Method: {1}; Type: {2}; Advance: {3}).", transactionId, methodName, typeName, advance.Advance));
                                this.LogWarn("Poller lost an advance (unknown progress transaction).");
                            }
                        }
                        var message = information as IProgressMessage;
                        if (message != null)
                        {
                            Debug.WriteLine(string.Format("Poller got a message (TransactionId: {0}; Method: {1}; Type: {2}; Message: {3} {4} {5}).", transactionId, methodName, typeName, message.Severity, message.Summary, message.Description));
                            if (progressTransaction != null)
                            {
                                progressTransaction.PostMessage(message);
                            }
                            else
                            {
                                Debug.WriteLine(string.Format("Poller lost a message (TransactionId: {0}; Method: {1}; Type: {2}; Message: {3} {4} {5}).", transactionId, methodName, typeName, message.Severity, message.Summary, message.Description));
                                this.LogWarn("Poller lost a message (unknown progress transaction).");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.LogInfo(ex, "Poller exception occured.");
                    Debug.WriteLine(string.Format("Poller exception occured (TransactionId: {0}; Method: {1}; Type: {2}; Exception: {3}).", transactionId, methodName, typeName, ex.Message));
                }

                try
                {
                    Thread.Sleep(pollerInterval);
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
            Debug.WriteLine(string.Format("Poller finished (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
        }

        #region ------ Internals: Disposable overrides --------------------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Finish();
            }
            Debug.WriteLine(string.Format("Poller disposed (TransactionId: {0}; Method: {1}; Type: {2}).", transactionId, methodName, typeName));
        }

        #endregion
    }
}