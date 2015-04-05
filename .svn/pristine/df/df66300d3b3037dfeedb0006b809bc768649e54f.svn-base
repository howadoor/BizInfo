using System;
using System.Diagnostics;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress notification facade.
    /// </summary>
    /// <remarks>
    /// See <see cref="IProgressTransaction"/> for more information.
    /// </remarks>
    public static class Progress
    {
        /// <summary>
        /// True if progress is activated for the current call context, false otherwise.
        /// </summary>
        /// <remarks>
        /// Progress is activated by calling one of the <see cref="Start"/> methods.
        /// The activation initializes new progress transaction and places its reference into
        /// thread static property <see cref="TopProgress"/>.
        /// </remarks>
        public static bool Active
        {
            get { return TopProgress != null; }
        }

        /// <summary>
        /// Default <see cref="IProgressService" /> port.
        /// </summary>
        public static int DefaultProgressServicePort { private get; set; }

        /// <summary>
        /// Default <see cref="IProgressService"/> URI.
        /// </summary>
        internal static Uri DefaultProgressServiceUri
        {
            get { return new UriBuilder("tcp", "localhost", DefaultProgressServicePort, typeof (IProgressService).Name).Uri; }
        }

        /// <summary>
        /// Current <see cref="IProgressService"/> URI.
        /// Setting a value takes host and port and constructs URI based on internal mask (tcp://host:port/IProgressService).
        /// </summary>
        public static Uri ProgressServiceUri
        {
            set
            {
                if (TopProgress != null && TopProgress is ProgressTransaction)
                {
                    Uri uri;
                    try
                    {
                        uri = new UriBuilder("tcp", value.Host, value.Port, typeof (IProgressService).Name).Uri;
                    }
                    catch (Exception)
                    {
                        uri = DefaultProgressServiceUri;
                    }
                    ((ProgressTransaction) TopProgress).ProgressServiceUri = uri;
                }
            }
            internal get { return TopProgress != null && TopProgress is ProgressTransaction ? ((ProgressTransaction) TopProgress).ProgressServiceUri : DefaultProgressServiceUri; }
        }

        /// <summary>
        /// Creates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName)
        {
            return Start(taskName, 100, null);
        }

        /// <summary>
        /// Creates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, IProgressNotification progressNotification)
        {
            return Start(taskName, 100, progressNotification);
        }

        /// <summary>
        /// Creates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, int allocatedScale, IProgressNotification progressNotification)
        {
            IProgressTransaction topProgress = TopProgress;
            int scaleMin = topProgress != null ? topProgress.ScaleMin : 0;
            int scaleMax = topProgress != null ? topProgress.ScaleMax : 100;

            return Start(taskName, scaleMin, scaleMax, allocatedScale, progressNotification);
        }

        /// <summary>
        /// Creates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, int scaleMin, int scaleMax)
        {
            return Start(taskName, scaleMin, scaleMax, 100);
        }

        /// <summary>
        /// Creates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, int scaleMin, int scaleMax, int allocatedScale)
        {
            return Start(taskName, scaleMin, scaleMax, allocatedScale, null);
        }

        /// <summary>
        /// Creates and starts new progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, int allocatedScale)
        {
            return Start(taskName, 0, 100, allocatedScale, null);
        }

        /// <summary>
        /// Crates and starts new root progress transaction.
        /// </summary>
        /// <remarks><see cref="IProgressTransaction"/></remarks>
        public static IProgressTransaction Start(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressNotification progressNotification)
        {
            IProgressTransaction topProgress = TopProgress;
            IProgressNotification notification = new CompoundProgressNotification(new[] {progressDisposer, progressNotification ?? new GlobalProgressNotification()});

            IProgressTransaction newTopProgress = topProgress != null
                                                      ? topProgress.CreateInnerTransaction(taskName, scaleMin, scaleMax, allocatedScale, notification)
                                                      : new ProgressTransaction(taskName, scaleMin, scaleMax, notification);

            TopProgress = newTopProgress;
            newTopProgress.Start();
            return newTopProgress;
        }

        /// <summary>
        /// <see cref="IProgressTransaction.Complete()"/>
        /// </summary>
        public static void Complete()
        {
            CheckProgressStarted();
            TopProgress.Complete();
        }

        /// <summary>
        /// <see cref="IProgressTransaction.Advance"/>
        /// </summary>
        /// <param name="value"></param>
        public static void Advance(int value)
        {
            CheckProgressStarted();
            TopProgress.Advance(value);
        }

        /// <summary>
        /// <see cref="IProgressTransaction.Progress"/>
        /// </summary>
        /// <returns></returns>
        public static int Get()
        {
            CheckProgressStarted();
            return TopProgress.Progress;
        }

        /// <summary>
        /// <see cref="IProgressTransaction.Progress"/>
        /// </summary>
        /// <returns></returns>
        public static void Set(int value)
        {
            CheckProgressStarted();
            TopProgress.Progress = value;
        }

        /// <summary>
        /// <see cref="IProgressTransaction.PostMessage"/>
        /// </summary>
        /// <returns></returns>
        public static void PostMessage(Exception ex)
        {
            PostMessage(new ProgressMessage(Severity.Error, ex, null));
        }

        /// <summary>
        /// <see cref="IProgressTransaction.PostMessage"/>
        /// </summary>
        /// <returns></returns>
        public static void PostMessage(Severity severity, string summary)
        {
            PostMessage(new ProgressMessage(severity, summary));
        }

        /// <summary>
        /// <see cref="IProgressTransaction.PostMessage"/>
        /// </summary>
        /// <returns></returns>
        public static void PostMessage(Severity severity, string summary, string description)
        {
            PostMessage(new ProgressMessage(severity, summary, description));
        }

        /// <summary>
        /// <see cref="IProgressTransaction.PostMessage"/>
        /// </summary>
        /// <returns></returns>
        public static void PostMessage(ProgressMessage message)
        {
            CheckProgressStarted();
            TopProgress.PostMessage(message);
        }

        /// <summary>
        /// Returns the current (top) progress transaction from the current thread's stack of progress transactions.
        /// </summary>
        /// <remarks>
        /// See <see cref="IProgressTransaction"/> for more info.
        /// </remarks>
        /// <returns></returns>
        public static IProgressTransaction GetTransaction()
        {
            return TopProgress;
        }

        /// <summary>
        /// Returns the progress service reference for some service URI passed as argument.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static IProgressService GetService(Uri uri)
        {
            return ((IProgressService) Activator.GetObject(typeof (IProgressService), uri.ToString()));
        }

        #region ------ Internals ------------------------------------------------------------------

        private const string progressNotStarted = "Progress has not been started";

        /// <summary>
        /// Handles disposal of finished progress transactions.
        /// </summary>
        private static readonly ProgressDisposer progressDisposer = new ProgressDisposer();

        [ThreadStatic] private static IProgressTransaction topProgress;

        #region ------ Progress Disposer ----------------------------------------------------------

        /// <summary>
        /// Handles disposal of finished progress transactions.
        /// </summary>
        private class ProgressDisposer : IProgressNotification
        {
            #region IProgressNotification Members

            public void OnProgressChanged(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressStarted(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressStopped(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressDisposed(IProgressTransaction progressTransaction)
            {
                Debug.Assert(progressTransaction == TopProgress);
                TopProgress = TopProgress.ParentTransaction;
            }

            public void OnProgressMessage(IProgressTransaction progressTransaction, IProgressMessage message)
            {
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// The current top progress transaction.
        /// </summary>
        public static IProgressTransaction TopProgress
        {
            get { return topProgress; }
            set { topProgress = value; }
        }

        private static void CheckProgressStarted()
        {
            if (TopProgress == null) throw new InvalidOperationException(progressNotStarted);
        }

        #endregion
    }

    #region ------ Unit tests ---------------------------------------------------------------------

#if NUNIT
    [TestFixture]
    public class ProgressTransactionTests
    {
        [Test]
        public void GenericTest()
        {
            using (Progress.Start("test", 0, 100, 100, new DummyProgressNotification()))
            {
                using (Progress.Start("foo1", 0, 100, 20, new DummyProgressNotification()))
                {
                    ProgressFoo1();
                }

                using (Progress.Start("foo2", 0, 100, 20, new DummyProgressNotification()))
                {
                    ProgressFoo2();
                }

                using (Progress.Start("foo3", 0, 100, 60, new DummyProgressNotification()))
                {
                    ProgressFoo3();
                }

                Assert.AreEqual(100, Progress.Get());
            }

            Assert.IsNull(Progress.GetTransaction());
        }

        private void ProgressFoo1()
        {
            using (Progress.Start("loop", -100, 100, 100, new DummyProgressNotification()))
            {
                for (int i = -100; i < 100; i++)
                {
                    Progress.Advance(1);
                }
            }
        }

        private void ProgressFoo2()
        {
            using (Progress.Start("loop", 0, 100, 100, new DummyProgressNotification()))
            {
                for (int i = 0; i < 100; i++)
                {
                    Progress.Advance(1);
                }
            }
        }

        private void ProgressFoo3()
        {
            using (Progress.Start("loop", 0, 200, 100, new DummyProgressNotification()))
            {
                for (int i = 0; i < 200; i++)
                {
                    Progress.Advance(1);
                }
            }
        }

        private class DummyProgressNotification : IProgressNotification
        {
            public void OnProgressChanged(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressStarted(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressStopped(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressDisposed(IProgressTransaction progressTransaction)
            {
            }

            public void OnProgressMessage(IProgressTransaction progressTransaction, IProgressMessage message)
            {
            }
        }
    }
#endif

    #endregion
}