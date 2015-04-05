using System;
using System.ComponentModel;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Component.Events
{
    /// <summary>
    /// Provides event invokation shortcuts for synchronized invoke of UI-related events.
    /// </summary>
    public static class EventInvoker
    {
        /// <summary>
        /// An application-provided defult provider of an <see cref="ISynchronizeInvoke"/> implementation.
        /// </summary>
        public static ISynchronizeInvokeAware DefaultSynchronizeInvokeProvider { get; set; }

        /// <summary>
        /// Gets a <see cref="ISynchronizeInvoke"/> implementation to be used when invoking event 
        /// handlers in the given object.
        /// </summary>
        /// <param name="obj">An object which itself is an <see cref="ISynchronizeInvoke"/> implementation.</param>
        /// <returns>The supplied <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is a null reference.</exception>
        public static ISynchronizeInvoke ToSynchronizeInvoke(this ISynchronizeInvoke obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return obj;
        }

        /// <summary>
        /// Gets a <see cref="ISynchronizeInvoke"/> implementation to be used when invoking event 
        /// handlers in the given object.
        /// </summary>
        /// <param name="obj">An object aware of an <see cref="ISynchronizeInvoke"/> implementation.</param>
        /// <returns>The provided <see cref="ISynchronizeInvoke"/> implementation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is a null reference.</exception>
        public static ISynchronizeInvoke ToSynchronizeInvoke(this ISynchronizeInvokeAware obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return obj.SynchronizeInvoke;
        }

        /// <summary>
        /// Gets a <see cref="ISynchronizeInvoke"/> implementation to be used when invoking event 
        /// handlers in the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The respective <see cref="ISynchronizeInvoke"/> implementation or a null reference.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is a null reference.</exception>
        public static ISynchronizeInvoke ToSynchronizeInvoke(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (obj is ISynchronizeInvoke) return (ISynchronizeInvoke) obj;
            if (obj is ISynchronizeInvokeAware) return ((ISynchronizeInvokeAware) obj).SynchronizeInvoke;
            return null;
        }

        /// <summary>
        /// Gets a <see cref="ISynchronizeInvoke"/> implementation corresponding to the given object,
        /// or the <see cref="DefaultSynchronizeInvokeProvider"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The respective <see cref="ISynchronizeInvoke"/> implementation, or the
        /// <see cref="DefaultSynchronizeInvokeProvider"/>, or a null reference when not available.</returns>
        public static ISynchronizeInvoke ToSynchronizeInvokeOrDefault(this object obj)
        {
            if (obj is ISynchronizeInvoke) return (ISynchronizeInvoke) obj;
            if (obj is ISynchronizeInvokeAware) return ((ISynchronizeInvokeAware) obj).SynchronizeInvoke;
            if (DefaultSynchronizeInvokeProvider != null) return DefaultSynchronizeInvokeProvider.SynchronizeInvoke;
            return null;
        }

        #region ------ Asynchronous invokation helpers --------------------------------------------

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is asynchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.BeginInvoke"/>); otherwise, the invoke is synchronous.
        /// </remarks>
        public static Action AsyncInvokeOf(this ISynchronizeInvoke control, Action onEvent)
        {
            return delegate
                       {
                           if (control.InvokeRequired)
                               control.BeginInvoke(onEvent, null);
                           else
                               onEvent();
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is asynchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.BeginInvoke"/>); otherwise, the invoke is synchronous.
        /// </remarks>
        public static Action<T> AsyncInvokeOf<T>(this ISynchronizeInvoke control, Action<T> onEvent)
        {
            return delegate(T obj)
                       {
                           if (control.InvokeRequired)
                               control.BeginInvoke(onEvent, new object[] {obj});
                           else
                               onEvent(obj);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is asynchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.BeginInvoke"/>); otherwise, the invoke is synchronous.
        /// </remarks>
        public static Action<T1, T2> AsyncInvokeOf<T1, T2>(this ISynchronizeInvoke control, Action<T1, T2> onEvent)
        {
            return delegate(T1 obj1, T2 obj2)
                       {
                           if (control.InvokeRequired)
                               control.BeginInvoke(onEvent, new object[] {obj1, obj2});
                           else
                               onEvent(obj1, obj2);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is asynchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.BeginInvoke"/>); otherwise, the invoke is synchronous.
        /// </remarks>
        public static Action<T1, T2, T3> AsyncInvokeOf<T1, T2, T3>(this ISynchronizeInvoke control, Action<T1, T2, T3> onEvent)
        {
            return delegate(T1 obj1, T2 obj2, T3 obj3)
                       {
                           if (control.InvokeRequired)
                               control.BeginInvoke(onEvent, new object[] {obj1, obj2, obj3});
                           else
                               onEvent(obj1, obj2, obj3);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is asynchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.BeginInvoke"/>); otherwise, the invoke is synchronous.
        /// </remarks>
        public static Action<T1, T2, T3, T4> AsyncInvokeOf<T1, T2, T3, T4>(this ISynchronizeInvoke control, Action<T1, T2, T3, T4> onEvent)
        {
            return delegate(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
                       {
                           if (control.InvokeRequired)
                               control.BeginInvoke(onEvent, new object[] {obj1, obj2, obj3, obj4});
                           else
                               onEvent(obj1, obj2, obj3, obj4);
                       };
        }

        #endregion

        #region ------ Synchronous invokation helpers ---------------------------------------------

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is synchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.Invoke"/>); otherwise, the invoke is synchronous
        /// as well.
        /// </remarks>
        public static Action SyncInvokeOf(this ISynchronizeInvoke control, Action onEvent)
        {
            return delegate
                       {
                           if (control.InvokeRequired)
                               control.Invoke(onEvent, null);
                           else
                               onEvent();
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is synchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.Invoke"/>); otherwise, the invoke is synchronous
        /// as well.
        /// </remarks>
        public static Action<T> SyncInvokeOf<T>(this ISynchronizeInvoke control, Action<T> onEvent)
        {
            return delegate(T obj)
                       {
                           if (control.InvokeRequired)
                               control.Invoke(onEvent, new object[] {obj});
                           else
                               onEvent(obj);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is synchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.Invoke"/>); otherwise, the invoke is synchronous
        /// as well.
        /// </remarks>
        public static Action<T1, T2> SyncInvokeOf<T1, T2>(this ISynchronizeInvoke control, Action<T1, T2> onEvent)
        {
            return delegate(T1 obj1, T2 obj2)
                       {
                           if (control.InvokeRequired)
                               control.Invoke(onEvent, new object[] {obj1, obj2});
                           else
                               onEvent(obj1, obj2);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is synchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.Invoke"/>); otherwise, the invoke is synchronous
        /// as well.
        /// </remarks>
        public static Action<T1, T2, T3> SyncInvokeOf<T1, T2, T3>(this ISynchronizeInvoke control, Action<T1, T2, T3> onEvent)
        {
            return delegate(T1 obj1, T2 obj2, T3 obj3)
                       {
                           if (control.InvokeRequired)
                               control.Invoke(onEvent, new object[] {obj1, obj2, obj3});
                           else
                               onEvent(obj1, obj2, obj3);
                       };
        }

        /// <summary>
        /// Returns a delegate the <paramref name="onEvent"/> method which is run on the supplied
        /// control's UI thread.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="control"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the method needs to be run on a different thread, the invoke is synchronous (i.e.
        /// uses <see cref="ISynchronizeInvoke.Invoke"/>); otherwise, the invoke is synchronous
        /// as well.
        /// </remarks>
        public static Action<T1, T2, T3, T4> SyncInvokeOf<T1, T2, T3, T4>(this ISynchronizeInvoke control, Action<T1, T2, T3, T4> onEvent)
        {
            return delegate(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
                       {
                           if (control.InvokeRequired)
                               control.Invoke(onEvent, new object[] {obj1, obj2, obj3, obj4});
                           else
                               onEvent(obj1, obj2, obj3, obj4);
                       };
        }

        #endregion

        // TODO Create AsyncInvokeOf and SyncInvokeOf overloads using ISychronizeInvokeAware.

/* TODO Wrong way. Needs to be done on invokation-time rather than binding-time.
   NOTE Isn't that bad; however, the returned delegate must always be of a pre-determined type, i.e. Action<object[]>, 
        and the user must supply arguments in an array and invoke using Delegate.DynamicInvoke.
 
        #region ------ Existing-delegate modification helpers -------------------------------------

        /// <summary>
        /// Modifies a delegate to behave like if it were created using one of the <see cref="AsyncInvokeOf"/> methods.
        /// </summary>
        /// <param name="onEvent">Delegate to be modified.</param>
        /// <returns>A new thread-aware delegate or the original delegate if its target doesn't 
        /// implement <see cref="ISynchronizeInvoke"/>.</returns>
        public static Delegate ToAsyncInvokeOf(Delegate onEvent)
        {
            // only care of objects capable of cross-thread invokation
            ISynchronizeInvoke target = onEvent.Target as ISynchronizeInvoke;
            if (target == null) return onEvent;

            // create thread-aware delegate
            DynamicDelegate dd = new DynamicDelegate {RealDelegate = onEvent, RealTarget = target};
            return Delegate.CreateDelegate(typeof(Action<object[]>), dd, DynamicDelegate.AsyncInvokeMethod);
        }

        /// <summary>
        /// Modifies a delegate to behave like if it were created using one of the <see cref="SyncInvokeOf"/> methods.
        /// </summary>
        /// <param name="onEvent">Delegate to be modified.</param>
        /// <returns>A new thread-aware delegate or the original delegate if its target doesn't 
        /// implement <see cref="ISynchronizeInvoke"/>.</returns>
        public static Delegate ToSyncInvokeOf(Delegate onEvent)
        {
            // only care of objects capable of cross-thread invokation
            ISynchronizeInvoke target = onEvent.Target as ISynchronizeInvoke;
            if (target == null) return onEvent;

            // create thread-aware delegate
            DynamicDelegate dd = new DynamicDelegate { RealDelegate = onEvent, RealTarget = target };
            return Delegate.CreateDelegate(typeof(Action<object[]>), dd, DynamicDelegate.SyncInvokeMethod);
        }

        public static Delegate ToAsyncInvoke(this ISynchronizeInvoke control, Delegate onEvent)
        {
            DynamicDelegate dd = new DynamicDelegate {RealTarget = control, RealDelegate = onEvent};
            return Delegate.CreateDelegate(typeof(Action<object[]>), dd, DynamicDelegate.AsyncInvokeMethod);
        }

        public static Delegate ToSyncInvoke(this ISynchronizeInvoke control, Delegate onEvent)
        {
            DynamicDelegate dd = new DynamicDelegate {RealTarget = control, RealDelegate = onEvent};
            return Delegate.CreateDelegate(typeof (Action<object[]>), dd, DynamicDelegate.SyncInvokeMethod);
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Provides invokation of a delegate on the delegate target's thread.
        /// </summary>
        private class DynamicDelegate
        {
            /// <summary>
            /// Metadata of the <see cref="AsyncInvoke"/> method.
            /// </summary>
            public static readonly MethodInfo AsyncInvokeMethod = typeof (DynamicDelegate).GetMethod("AsyncInvoke");

            /// <summary>
            /// Metadata of the <see cref="SyncInvoke"/> method.
            /// </summary>
            public static readonly MethodInfo SyncInvokeMethod = typeof (DynamicDelegate).GetMethod("SyncInvoke");

            /// <summary>
            /// The real target of invokation.
            /// </summary>
            public ISynchronizeInvoke RealTarget { get; set; }

            /// <summary>
            /// The real delegate to be invoked.
            /// </summary>
            public Delegate RealDelegate { get; set; }

            /// <summary>
            /// Performs asynchronous (if required) invokation of the <see cref="RealDelegate"/> 
            /// on the <see cref="RealTarget"/>'s thread.
            /// </summary>
            /// <param name="args"></param>
            public void AsyncInvoke(params object[] args)
            {
                if (RealTarget.InvokeRequired)
                    RealTarget.BeginInvoke(RealDelegate, args);
                else
                    RealDelegate.DynamicInvoke(args);
            }

            /// <summary>
            /// Performs synchronous (if required) invokation of the <see cref="RealDelegate"/> 
            /// on the <see cref="RealTarget"/>'s thread.
            /// </summary>
            /// <param name="args"></param>
            public void SyncInvoke(params object[] args)
            {
                if (RealTarget.InvokeRequired)
                    RealTarget.Invoke(RealDelegate, args);
                else
                    RealDelegate.DynamicInvoke(args);
            }

            #region ------ Internals: Objects overrides -------------------------------------------

            public override int GetHashCode()
            {
                return RealDelegate.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                DynamicDelegate d = obj as DynamicDelegate;
                if (d == null) return false;
                return RealDelegate.Equals(d.RealDelegate);
            }

            #endregion
        }

        #endregion
*/
    }

/*

    #region ------ Unit tests of the EventInvoker class -------------------------------------------

    #if NUNIT

    /// <summary>
    /// Unit tests of the <see cref="EventInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class EventInvoker_Tests
    {
        private class Target
        {
            public int x;

            public void Method(int x)
            {
                this.x = x;
            }
        }

        private class Target2 : Target, ISynchronizeInvoke
        {
            public bool? async;

            public IAsyncResult BeginInvoke(Delegate method, object[] args)
            {
                async = true;
                method.DynamicInvoke(args);
                return null;
            }

            public object EndInvoke(IAsyncResult result)
            {
                return null;
            }

            public object Invoke(Delegate method, object[] args)
            {
                async = false;
                method.DynamicInvoke(args);
                return null;
            }

            public bool InvokeRequired
            {
                get { return true; }
            }
        }

        /// <summary>
        /// Tests invoking the original delegate via <see cref="EventInvoker.ToSyncInvokeOf"/> and <see cref="EventInvoker.ToAsyncInvokeOf"/>.
        /// </summary>
        [Test]
        public void TestOriginalDelegate()
        {
            Target t = new Target();

            Delegate m1 = EventInvoker.ToSyncInvokeOf(new Action<int>(t.Method));
            m1.DynamicInvoke(10);
            Assert.AreEqual(10, t.x);

            Delegate m2 = EventInvoker.ToAsyncInvokeOf(new Action<int>(t.Method));
            m2.DynamicInvoke(20);
            Assert.AreEqual(20, t.x);
        }

        /// <summary>
        /// Tests invoking the new delegate via <see cref="EventInvoker.ToSyncInvokeOf"/> and <see cref="EventInvoker.ToAsyncInvokeOf"/>.
        /// </summary>
        [Test]
        public void TestNewDelegate()
        {
            Target2 t = new Target2();

            Delegate m1 = EventInvoker.ToSyncInvokeOf(new Action<int>(t.Method));
            m1.DynamicInvoke(10);
            Assert.AreEqual(10, t.x);
            Assert.AreEqual(false, t.async);

            Delegate m2 = EventInvoker.ToAsyncInvokeOf(new Action<int>(t.Method));
            m2.DynamicInvoke(20);
            Assert.AreEqual(20, t.x);
            Assert.AreEqual(true, t.async);
        }
    }

    #endif

    #endregion
*/
}