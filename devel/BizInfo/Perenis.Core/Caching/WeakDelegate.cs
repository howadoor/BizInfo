using System;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// Static class used to convert real (strong) delegates into weak
    /// delegates.
    /// </summary>
    public static class WeakDelegate
    {
        #region IsWeakDelegate

        /// <summary>
        /// Verifies if a handler is already a weak delegate.
        /// </summary>
        /// <param name="handler">The handler to verify.</param>
        /// <returns>true if the handler is already a weak delegate, false otherwise.</returns>
        public static bool IsWeakDelegate(Delegate handler)
        {
            return handler.Target != null && handler.Target is WeakDelegateBase;
        }

        #endregion

        #region From(Action)

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static Action From(Action strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakActionWrapper(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action handler) :
                base(handler)
            {
            }

            internal void Execute()
            {
                Invoke(null);
            }
        }

        #endregion

        #region From(Action<TValue>)

        /// <summary>
        /// Creates a weak delegate from an Action&lt;TValue&gt; delegate.
        /// </summary>
        public static Action<T> From<T>(Action<T> strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakActionWrapper<T>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper<T> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T> handler) :
                base(handler)
            {
            }

            internal void Execute(T parameter)
            {
                Invoke(new object[] {parameter});
            }
        }

        #endregion

        #region From(Action<T1, T2>)

        /// <summary>
        /// Creates a weak delegate from an Action&lt;T1, T2&gt; delegate.
        /// </summary>
        public static Action<T1, T2> From<T1, T2>(Action<T1, T2> strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakActionWrapper<T1, T2>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper<T1, T2> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2)
            {
                Invoke(new object[] {parameter1, parameter2});
            }
        }

        #endregion

        #region From(Action<T1, T2, T3>)

        /// <summary>
        /// Creates a weak delegate from an Action&lt;T1, T2, T3&gt; delegate.
        /// </summary>
        public static Action<T1, T2, T3> From<T1, T2, T3>(Action<T1, T2, T3> strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakActionWrapper<T1, T2, T3>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper<T1, T2, T3> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2, T3> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2, T3 parameter3)
            {
                Invoke(new object[] {parameter1, parameter2, parameter3});
            }
        }

        #endregion

        #region From(Action<T1, T2, T3, T4>)

        /// <summary>
        /// Creates a weak delegate from an Action&lt;T1, T2, T3, T4&gt; delegate.
        /// </summary>
        public static Action<T1, T2, T3, T4> From<T1, T2, T3, T4>(Action<T1, T2, T3, T4> strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakActionWrapper<T1, T2, T3, T4>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper<T1, T2, T3, T4> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2, T3, T4> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
            {
                Invoke(new object[] {parameter1, parameter2, parameter3, parameter4});
            }
        }

        #endregion

        #region From(EventHandler)

        /// <summary>
        /// Creates a weak delegate from an EventHandler delegate.
        /// </summary>
        public static EventHandler From(EventHandler strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakEventHandlerWrapper(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakEventHandlerWrapper :
            WeakDelegateBase
        {
            internal WeakEventHandlerWrapper(EventHandler handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, EventArgs e)
            {
                Invoke(new[] {sender, e});
            }
        }

        #endregion

        #region From(EventHandler<TEventArgs>)

        /// <summary>
        /// Creates a weak delegate from an Action&lt;TEventArgs&gt; delegate.
        /// </summary>
        public static EventHandler<TEventArgs> From<TEventArgs>(EventHandler<TEventArgs> strongHandler)
            where
                TEventArgs : EventArgs
        {
            if (IsWeakDelegate(strongHandler))
                throw new ArgumentException("Your delegate is already a weak-delegate.");

            var wrapper = new WeakEventHandlerWrapper<TEventArgs>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakEventHandlerWrapper<TEventArgs> :
            WeakDelegateBase
            where
                TEventArgs : EventArgs
        {
            internal WeakEventHandlerWrapper(EventHandler<TEventArgs> handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, TEventArgs e)
            {
                Invoke(new[] {sender, e});
            }
        }

        #endregion
    }
}