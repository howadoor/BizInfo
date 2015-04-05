using System;
using System.Collections.Generic;
using System.Windows;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Provides a strongly typed facase of the <see cref="WeakEventManager"/>.
    /// </summary>
    /// <typeparam name="TManager">The actual type of the event manager.</typeparam>
    /// <typeparam name="TInterface">The interface of the event source.</typeparam>
    /// <remarks>
    /// To implement an actual weak event manager, derive from this class, and 
    /// attach / detach event handlers to the events being managed in the
    /// <see cref="StartListening(TInterface)"/> and <see cref="StopListening(TInterface)"/>.
    /// </remarks>
    public abstract class WeakEventManager<TManager, TInterface> : WeakEventManager
        where TManager : WeakEventManager<TManager, TInterface>, new()
    {
        /// <summary>
        /// Registers a <paramref name="listener"/> to consume events events from the 
        /// <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The source of events.</param>
        /// <param name="listener">The object listening for the events.</param>
        public static void AddListener(TInterface source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Unregisters a <paramref name="listener"/> that consumes events from the 
        /// <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The source of events.</param>
        /// <param name="listener">The object listening for the events.</param>
        public static void RemoveListener(TInterface source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <summary>
        /// Starts listening for events produced by the given <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The source of events.</param>
        protected abstract void StartListening(TInterface source);

        /// <summary>
        /// Stops listening for events produced by the given <paramref name="source"/> object.
        /// </summary>
        /// <param name="source">The source of events.</param>
        protected abstract void StopListening(TInterface source);

        #region ------ Internals ------------------------------------------------------------------

        private static readonly Dictionary<Type, WeakEventManager> Managers = new Dictionary<Type, WeakEventManager>();

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        private static TManager CurrentManager
        {
            get
            {
                Type managerType = typeof (TManager);
                if (!Managers.ContainsKey(managerType))
                {
                    Managers[managerType] = new TManager();
                }
                return (TManager) Managers[managerType];
            }
        }

        protected override void StartListening(object source)
        {
            StartListening((TInterface) source);
        }

        protected override void StopListening(object source)
        {
            StopListening((TInterface) source);
        }

        #endregion
    }
}