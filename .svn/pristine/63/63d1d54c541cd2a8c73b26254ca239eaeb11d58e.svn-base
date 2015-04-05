using System;
using System.Collections.Generic;
using System.Threading;
using Perenis.Core.Caching;

namespace Perenis.Core.General
{
    /// <summary>
    /// This is a thread pool that, different from System.Threading.ThreadPool,
    /// does not have a thread limit and the threads are not background while
    /// they run. Comparing to the system ThreadPool, it is better if the
    /// thread can enter in wait mode. This happens when one thread has a 
    /// "fast" process, but can only terminate after another thread returns.
    /// 
    /// This thread pool keep threads alive for a certain number of generations.
    /// The default value is two. So, at the first use, it lives for one more
    /// generation. After the second use in that generation, it is marked to 
    /// live for two generations. Subsequent uses in this generation will not
    /// change anything. Do not use very high values, as this can cause memory
    /// usage problems.
    /// </summary>
    public static class UnlimitedThreadPool
    {
        #region Private fields

        private static readonly object fLock = new object();
        private static volatile List<Parameters> fFreeEvents = new List<Parameters>();

        #endregion

        #region Constructor

        static UnlimitedThreadPool()
        {
            GCUtils.Collected += p_Collected;
        }

        #endregion

        #region p_Collected

        private static void p_Collected()
        {
            GCUtils.Collected += p_Collected;

            int minimumCollectionNumber = GC.CollectionCount(GC.MaxGeneration);
            List<Parameters> freeEvents;
            lock (fLock)
            {
                freeEvents = fFreeEvents;

                var newFreeEvents = new List<Parameters>();
                fFreeEvents = newFreeEvents;

                foreach (Parameters p in freeEvents)
                {
                    if (p.KeepAliveUntilCollectionOfNumber >= minimumCollectionNumber)
                        newFreeEvents.Add(p);
                    else
                    {
                        p.Run = null;
                        p.WaitEvent.Set();
                    }
                }
            }
        }

        #endregion

        #region Properties

        #region CollectionsToKeepAlive

        private static volatile int fCollectionsToKeepAlive = 2;

        /// <summary>
        /// Gets or sets the maximum number of collections to keep a Thread from this pool
        /// alive. The default value is two.
        /// </summary>
        public static int CollectionsToKeepAlive
        {
            get { return fCollectionsToKeepAlive; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("value must be greater than zero.", "UnlimitedThreadPool.CollectionsToKeepAlive");

                fCollectionsToKeepAlive = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Run

        /// <summary>
        /// Queues an user item. In fact, the item will be executed in a new thread if no available thread
        /// exists.
        /// </summary>
        /// <param name="handler">The function to execute.</param>
        public static void Run(ParameterizedThreadStart handler)
        {
            Run(handler, null);
        }

        /// <summary>
        /// Queues an user item. In fact, the item will be executed in a new thread if no available thread
        /// exists.
        /// </summary>
        /// <param name="handler">The function to execute.</param>
        /// <param name="state">The object passed as parameter to the function.</param>
        public static void Run(ParameterizedThreadStart handler, object state)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            Parameters p = null;
            lock (fLock)
            {
                int count = fFreeEvents.Count;
                if (count > 0)
                {
                    int index = count - 1;
                    p = fFreeEvents[index];
                    fFreeEvents.RemoveAt(index);
                }
            }

            if (p == null)
            {
                p = new Parameters();
                p.WaitEvent = new ManualResetEvent(false);
                var thread = new Thread(p_RunThread);
                p.Thread = thread;
                thread.Start(p);
            }

            p.Run = handler;
            p.State = state;

            p.Thread.IsBackground = false;
            p.WaitEvent.Set();
        }

        #endregion

        #region p_RunThread

        private static void p_RunThread(object parameters)
        {
            Thread currentThread = Thread.CurrentThread;
            var p = (Parameters) parameters;
            ManualResetEvent waitEvent = p.WaitEvent;

            try
            {
                while (true)
                {
                    waitEvent.WaitOne();

                    if (p.Run == null)
                    {
                        waitEvent.Close();
                        return;
                    }

                    p.Run(p.State);

                    lock (fLock)
                    {
                        int actualCollectionNumber = GC.CollectionCount(GC.MaxGeneration);
                        int keepAliveUntil = p.KeepAliveUntilCollectionOfNumber;

                        if (keepAliveUntil <= actualCollectionNumber)
                            p.KeepAliveUntilCollectionOfNumber = actualCollectionNumber + 1;
                        else
                        {
                            int maxValue = actualCollectionNumber + fCollectionsToKeepAlive;
                            if (keepAliveUntil < maxValue)
                                p.KeepAliveUntilCollectionOfNumber = keepAliveUntil + 1;
                        }

                        currentThread.IsBackground = true;
                        waitEvent.Reset();
                        fFreeEvents.Add(p);
                    }
                }
            }
            finally
            {
                if (currentThread.IsBackground)
                    lock (fLock)
                        fFreeEvents.Remove(p);
            }
        }

        #endregion

        #endregion

        #region Parameters - Nested class

        private sealed class Parameters
        {
            internal volatile int KeepAliveUntilCollectionOfNumber;
            internal volatile ParameterizedThreadStart Run;
            internal volatile object State;
            internal Thread Thread;
            internal ManualResetEvent WaitEvent;
        }

        #endregion
    }
}