using System;
using System.Windows.Threading;

namespace Perenis.Core.Component.Events
{
    public static class DispatcherEx
    {
        public static void BeginInvoke(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
            {
                action();
                return;
            }
            dispatcher.BeginInvoke(action);
        }

        public static bool SynchronizedBeginInvoke(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess()) return true;
            dispatcher.BeginInvoke(action);
            return false;
        }

        public static bool SynchronizedBeginInvoke<T1>(this Dispatcher dispatcher, Action<T1> action, T1 arg1)
        {
            if (dispatcher.CheckAccess()) return true;
            dispatcher.BeginInvoke(action, arg1);
            return false;
        }

        public static bool SynchronizedBeginInvoke<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (dispatcher.CheckAccess()) return true;
            dispatcher.BeginInvoke(action, arg1, arg2);
            return false;
        }

        public static bool SynchronizedBeginInvoke<T1, T2, T3>(this Dispatcher dispatcher, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (dispatcher.CheckAccess()) return true;
            dispatcher.BeginInvoke(action, arg1, arg2, arg3);
            return false;
        }

        public static T SynchronizedInvoke<T>(this Dispatcher dispatcher, Func<T> function)
        {
            if (dispatcher.CheckAccess()) return function();
            return (T) dispatcher.Invoke(function);
        }
    }
}