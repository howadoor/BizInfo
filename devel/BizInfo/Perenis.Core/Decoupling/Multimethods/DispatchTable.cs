using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Perenis.Core.Decoupling.Multimethods
{
    public class DispatchTable<TInvoker>
    {
        private readonly int _argumentCount;
        private readonly bool _is32BitPlatform;
        private volatile IDictionary _matches; // copy on write SortedList<types[], TInvoker> optimized for #args

        public DispatchTable(int argumentCount, bool is32BitPlatform)
        {
            _argumentCount = argumentCount;
            _is32BitPlatform = is32BitPlatform;

            if (_is32BitPlatform)
            {
                if (_argumentCount == 1)
                    _matches = new SortedList<IntKey1, TInvoker>();
                else if (_argumentCount == 2)
                    _matches = new SortedList<IntKey2, TInvoker>();
                else if (_argumentCount == 3)
                    _matches = new SortedList<IntKey3, TInvoker>();
                else if (_argumentCount == 4)
                    _matches = new SortedList<IntKey4, TInvoker>();
            }
            else
            {
                if (_argumentCount == 1)
                    _matches = new SortedList<LongKey1, TInvoker>();
                else if (_argumentCount == 2)
                    _matches = new SortedList<LongKey2, TInvoker>();
                else if (_argumentCount == 3)
                    _matches = new SortedList<LongKey3, TInvoker>();
                else if (_argumentCount == 4)
                    _matches = new SortedList<LongKey4, TInvoker>();
            }

            if (_matches == null)
            {
                throw new ArgumentException("Expecting a value between 1 and 4", "argumentCount");
            }
        }

        public int ArgumentCount
        {
            get { return _argumentCount; }
        }

        public int Size
        {
            get { return _matches.Count; }
        }

        public TInvoker Match(object[] args)
        {
            return _is32BitPlatform ? MatchIntKey(args) : MatchLongKey(args);
        }

        public TInvoker MatchFunction(Type returnType, object[] args)
        {
            return _is32BitPlatform ? MatchIntKey(returnType, args) : MatchLongKey(returnType, args);
        }

        public void Add(object[] args, TInvoker target)
        {
            if (_is32BitPlatform)
            {
                AddIntKey(args, target);
            }
            else
            {
                AddLongKey(args, target);
            }
        }

        public void AddFunction(Type returnType, object[] args, TInvoker target)
        {
            if (_is32BitPlatform)
            {
                AddIntKey(returnType, args, target);
            }
            else
            {
                AddLongKey(returnType, args, target);
            }
        }

        // non thread safe
        internal void InternalAdd(Type[] args, TInvoker target)
        {
            if (_is32BitPlatform)
            {
                AddIntKeyInPlace(args, target);
            }
            else
            {
                AddLongKey_InPlace(args, target);
            }
        }

        #region Add Key

        // non thread safe
        private void AddKey_InPlace<TKey>(TKey key, TInvoker value)
        {
            var list = _matches as SortedList<TKey, TInvoker>;
            if (!list.ContainsKey(key))
            {
                list[key] = value;
            }
            else
            {
                throw new DispatchTableConflictException(key, string.Format("Cannot add {1}, there is already {2} for the same key {0}", key, value, list[key]));
            }
        }

        // thread safe: updates a copy of _matches and then assigns copy to _matches
        private void AddKey<TKey>(TKey key, TInvoker target)
            where TKey : IComparable<TKey>
        {
            var current = (SortedList<TKey, TInvoker>) _matches;
            var copy =
                new SortedList<TKey, TInvoker>(current.Count + 1);

            bool added = false;

            foreach (var pair in current)
            {
                int c = 0;

                if (!added && (c = key.CompareTo(pair.Key)) <= 0)
                {
                    added = true;
                    if (c != 0)
                    {
                        copy.Add(key, target);
                    }
                }

                copy.Add(pair.Key, pair.Value);
            }

            if (!added)
            {
                copy.Add(key, target);
            }

            _matches = copy;
        }

        #endregion

        #region Int32 keys

        private TInvoker MatchIntKey(object[] args)
        {
            TInvoker target;
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new IntKey1(args);
                    ((SortedList<IntKey1, TInvoker>) _matches).TryGetValue(key1, out target);
                    break;
                case 2:
                    var key2 = new IntKey2(args);
                    ((SortedList<IntKey2, TInvoker>) _matches).TryGetValue(key2, out target);
                    break;
                case 3:
                    var key3 = new IntKey3(args);
                    ((SortedList<IntKey3, TInvoker>) _matches).TryGetValue(key3, out target);
                    break;
                case 4:
                    var key4 = new IntKey4(args);
                    ((SortedList<IntKey4, TInvoker>) _matches).TryGetValue(key4, out target);
                    break;
                default:
                    throw new InvalidOperationException("Wrong arguments count set for dispatch table. Dispatch table supports 1-4 arguments.");
            }
            return target;
        }

        private TInvoker MatchIntKey(Type returnType, object[] args)
        {
            TInvoker target;
            switch (_argumentCount - 1)
            {
                case 0:
                    var key1 = new IntKey1(returnType);
                    ((SortedList<IntKey1, TInvoker>) _matches).TryGetValue(key1, out target);
                    break;
                case 1:
                    var key2 = new IntKey2(returnType, args);
                    ((SortedList<IntKey2, TInvoker>) _matches).TryGetValue(key2, out target);
                    break;
                case 2:
                    var key3 = new IntKey3(returnType, args);
                    ((SortedList<IntKey3, TInvoker>) _matches).TryGetValue(key3, out target);
                    break;
                case 3:
                    var key4 = new IntKey4(returnType, args);
                    ((SortedList<IntKey4, TInvoker>) _matches).TryGetValue(key4, out target);
                    break;
                default:
                    throw new InvalidOperationException("Wrong arguments count set for dispatch table. Dispatch table supports 1-4 arguments.");
            }
            return target;
        }

        private void AddIntKeyInPlace(Type[] args, TInvoker target)
        {
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new IntKey1(args);
                    AddKey_InPlace(key1, target);
                    break;
                case 2:
                    var key2 = new IntKey2(args);
                    AddKey_InPlace(key2, target);
                    break;
                case 3:
                    var key3 = new IntKey3(args);
                    AddKey_InPlace(key3, target);
                    break;
                case 4:
                    var key4 = new IntKey4(args);
                    AddKey_InPlace(key4, target);
                    break;
                default:
                    throw new InvalidOperationException("Wrong arguments count set for dispatch table. Dispatch table supports 1-4 arguments.");
            }
        }

        private void AddIntKey(object[] args, TInvoker target)
        {
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new IntKey1(args);
                    AddKey(key1, target);
                    break;
                case 2:
                    var key2 = new IntKey2(args);
                    AddKey(key2, target);
                    break;
                case 3:
                    var key3 = new IntKey3(args);
                    AddKey(key3, target);
                    break;
                case 4:
                    var key4 = new IntKey4(args);
                    AddKey(key4, target);
                    break;
                default:
                    throw new InvalidOperationException("Wrong arguments count set for dispatch table. Dispatch table supports 1-4 arguments.");
            }
        }

        private void AddIntKey(Type returnType, object[] args, TInvoker target)
        {
            switch (_argumentCount - 1)
            {
                case 0:
                    var key1 = new IntKey1(returnType);
                    AddKey(key1, target);
                    break;
                case 1:
                    var key2 = new IntKey2(returnType, args);
                    AddKey(key2, target);
                    break;
                case 2:
                    var key3 = new IntKey3(returnType, args);
                    AddKey(key3, target);
                    break;
                case 3:
                    var key4 = new IntKey4(returnType, args);
                    AddKey(key4, target);
                    break;
                default:
                    throw new InvalidOperationException("Wrong arguments count set for dispatch table. Dispatch table supports 1-4 arguments.");
            }
        }

        #region Nested type: IntKey1

        private struct IntKey1 : IComparable<IntKey1>
        {
            private readonly int _arg1Type;

            public IntKey1(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt32();
            }

            public IntKey1(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt32();
            }

            public IntKey1(Type returnType)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt32();
            }

            #region IComparable<DispatchTable<TInvoker>.IntKey1> Members

            public int CompareTo(IntKey1 other)
            {
                return _arg1Type.CompareTo(other._arg1Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}]", GetType(), _arg1Type);
            }
        }

        #endregion

        #region Nested type: IntKey2

        private struct IntKey2 : IComparable<IntKey2>
        {
            private readonly int _arg1Type;
            private readonly int _arg2Type;

            public IntKey2(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt32();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt32();
            }

            public IntKey2(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt32();
                _arg2Type = args[1].TypeHandle.Value.ToInt32();
            }

            public IntKey2(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt32();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt32();
            }

            #region IComparable<DispatchTable<TInvoker>.IntKey2> Members

            public int CompareTo(IntKey2 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                return _arg2Type.CompareTo(other._arg2Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}]", GetType(), _arg1Type, _arg2Type);
            }
        }

        #endregion

        #region Nested type: IntKey3

        private struct IntKey3 : IComparable<IntKey3>
        {
            private readonly int _arg1Type;
            private readonly int _arg2Type;
            private readonly int _arg3Type;

            public IntKey3(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt32();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt32();
                _arg3Type = args[2].GetType().TypeHandle.Value.ToInt32();
            }

            public IntKey3(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt32();
                _arg2Type = args[1].TypeHandle.Value.ToInt32();
                _arg3Type = args[2].TypeHandle.Value.ToInt32();
            }

            public IntKey3(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt32();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt32();
                _arg3Type = args[1].GetType().TypeHandle.Value.ToInt32();
            }

            #region IComparable<DispatchTable<TInvoker>.IntKey3> Members

            public int CompareTo(IntKey3 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                result = _arg2Type.CompareTo(other._arg2Type);

                if (result != 0)
                    return result;

                return _arg3Type.CompareTo(other._arg3Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}; {3}]", GetType(), _arg1Type, _arg2Type, _arg3Type);
            }
        }

        #endregion

        #region Nested type: IntKey4

        private struct IntKey4 : IComparable<IntKey4>
        {
            private readonly int _arg1Type;
            private readonly int _arg2Type;
            private readonly int _arg3Type;
            private readonly int _arg4Type;

            public IntKey4(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt32();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt32();
                _arg3Type = args[2].GetType().TypeHandle.Value.ToInt32();
                _arg4Type = args[3].GetType().TypeHandle.Value.ToInt32();
            }

            public IntKey4(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt32();
                _arg2Type = args[1].TypeHandle.Value.ToInt32();
                _arg3Type = args[2].TypeHandle.Value.ToInt32();
                _arg4Type = args[3].TypeHandle.Value.ToInt32();
            }

            public IntKey4(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt32();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt32();
                _arg3Type = args[1].GetType().TypeHandle.Value.ToInt32();
                _arg4Type = args[2].GetType().TypeHandle.Value.ToInt32();
            }

            #region IComparable<DispatchTable<TInvoker>.IntKey4> Members

            public int CompareTo(IntKey4 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                result = _arg2Type.CompareTo(other._arg2Type);

                if (result != 0)
                    return result;

                result = _arg3Type.CompareTo(other._arg3Type);

                if (result != 0)
                    return result;

                return _arg4Type.CompareTo(other._arg4Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}; {3}; {4}]", GetType(), _arg1Type, _arg2Type, _arg3Type, _arg4Type);
            }
        }

        #endregion

        #endregion

        #region Int64 keys

        private TInvoker MatchLongKey(object[] args)
        {
            TInvoker target = default(TInvoker);
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new LongKey1(args);
                    (_matches as SortedList<LongKey1, TInvoker>).TryGetValue(key1, out target);
                    break;
                case 2:
                    var key2 = new LongKey2(args);
                    (_matches as SortedList<LongKey2, TInvoker>).TryGetValue(key2, out target);
                    break;
                case 3:
                    var key3 = new LongKey3(args);
                    (_matches as SortedList<LongKey3, TInvoker>).TryGetValue(key3, out target);
                    break;
                default:
                    var key4 = new LongKey4(args);
                    (_matches as SortedList<LongKey4, TInvoker>).TryGetValue(key4, out target);
                    break;
            }
            return target;
        }

        private TInvoker MatchLongKey(Type returnType, object[] args)
        {
            TInvoker target = default(TInvoker);
            switch (_argumentCount - 1)
            {
                case 0:
                    var key1 = new LongKey1(returnType);
                    (_matches as SortedList<LongKey1, TInvoker>).TryGetValue(key1, out target);
                    break;
                case 1:
                    var key2 = new LongKey2(returnType, args);
                    (_matches as SortedList<LongKey2, TInvoker>).TryGetValue(key2, out target);
                    break;
                case 2:
                    var key3 = new LongKey3(returnType, args);
                    (_matches as SortedList<LongKey3, TInvoker>).TryGetValue(key3, out target);
                    break;
                default:
                    var key4 = new LongKey4(returnType, args);
                    (_matches as SortedList<LongKey4, TInvoker>).TryGetValue(key4, out target);
                    break;
            }
            return target;
        }

        private void AddLongKey_InPlace(Type[] args, TInvoker target)
        {
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new LongKey1(args);
                    AddKey_InPlace(key1, target);
                    break;
                case 2:
                    var key2 = new LongKey2(args);
                    AddKey_InPlace(key2, target);
                    break;
                case 3:
                    var key3 = new LongKey3(args);
                    AddKey_InPlace(key3, target);
                    break;
                default:
                    var key4 = new LongKey4(args);
                    AddKey_InPlace(key4, target);
                    break;
            }
        }

        private void AddLongKey(object[] args, TInvoker target)
        {
            switch (_argumentCount)
            {
                case 1:
                    var key1 = new LongKey1(args);
                    AddKey(key1, target);
                    break;
                case 2:
                    var key2 = new LongKey2(args);
                    AddKey(key2, target);
                    break;
                case 3:
                    var key3 = new LongKey3(args);
                    AddKey(key3, target);
                    break;
                default:
                    var key4 = new LongKey4(args);
                    AddKey(key4, target);
                    break;
            }
        }

        private void AddLongKey(Type returnType, object[] args, TInvoker target)
        {
            switch (_argumentCount - 1)
            {
                case 0:
                    var key1 = new LongKey1(returnType);
                    AddKey(key1, target);
                    break;
                case 1:
                    var key2 = new LongKey2(returnType, args);
                    AddKey(key2, target);
                    break;
                case 2:
                    var key3 = new LongKey3(returnType, args);
                    AddKey(key3, target);
                    break;
                default:
                    var key4 = new LongKey4(returnType, args);
                    AddKey(key4, target);
                    break;
            }
        }

        #region Nested type: LongKey1

        private struct LongKey1 : IComparable<LongKey1>
        {
            private readonly long _arg1Type;

            public LongKey1(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt64();
            }

            public LongKey1(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt64();
            }

            public LongKey1(Type returnType)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt64();
            }

            #region IComparable<DispatchTable<TInvoker>.LongKey1> Members

            public int CompareTo(LongKey1 other)
            {
                return _arg1Type.CompareTo(other._arg1Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}]", GetType(), _arg1Type);
            }
        }

        #endregion

        #region Nested type: LongKey2

        private struct LongKey2 : IComparable<LongKey2>
        {
            private readonly long _arg1Type;
            private readonly long _arg2Type;

            public LongKey2(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt64();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt64();
            }

            public LongKey2(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt64();
                _arg2Type = args[1].TypeHandle.Value.ToInt64();
            }

            public LongKey2(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt64();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt64();
            }

            #region IComparable<DispatchTable<TInvoker>.LongKey2> Members

            public int CompareTo(LongKey2 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                return _arg2Type.CompareTo(other._arg2Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}]", GetType(), _arg1Type, _arg2Type);
            }
        }

        #endregion

        #region Nested type: LongKey3

        private struct LongKey3 : IComparable<LongKey3>
        {
            private readonly long _arg1Type;
            private readonly long _arg2Type;
            private readonly long _arg3Type;

            public LongKey3(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt64();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt64();
                _arg3Type = args[2].GetType().TypeHandle.Value.ToInt64();
            }

            public LongKey3(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt64();
                _arg2Type = args[1].TypeHandle.Value.ToInt64();
                _arg3Type = args[2].TypeHandle.Value.ToInt64();
            }

            public LongKey3(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt64();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt64();
                _arg3Type = args[1].GetType().TypeHandle.Value.ToInt64();
            }

            #region IComparable<DispatchTable<TInvoker>.LongKey3> Members

            public int CompareTo(LongKey3 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                result = _arg2Type.CompareTo(other._arg2Type);

                if (result != 0)
                    return result;

                return _arg3Type.CompareTo(other._arg3Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}; {3}]", GetType(), _arg1Type, _arg2Type, _arg3Type);
            }
        }

        #endregion

        #region Nested type: LongKey4

        private struct LongKey4 : IComparable<LongKey4>
        {
            private readonly long _arg1Type;
            private readonly long _arg2Type;
            private readonly long _arg3Type;
            private readonly long _arg4Type;

            public LongKey4(object[] args)
            {
                _arg1Type = args[0].GetType().TypeHandle.Value.ToInt64();
                _arg2Type = args[1].GetType().TypeHandle.Value.ToInt64();
                _arg3Type = args[2].GetType().TypeHandle.Value.ToInt64();
                _arg4Type = args[3].GetType().TypeHandle.Value.ToInt64();
            }

            public LongKey4(Type[] args)
            {
                _arg1Type = args[0].TypeHandle.Value.ToInt64();
                _arg2Type = args[1].TypeHandle.Value.ToInt64();
                _arg3Type = args[2].TypeHandle.Value.ToInt64();
                _arg4Type = args[3].TypeHandle.Value.ToInt64();
            }

            public LongKey4(Type returnType, object[] args)
            {
                _arg1Type = returnType.TypeHandle.Value.ToInt64();
                _arg2Type = args[0].GetType().TypeHandle.Value.ToInt64();
                _arg3Type = args[1].GetType().TypeHandle.Value.ToInt64();
                _arg4Type = args[2].GetType().TypeHandle.Value.ToInt64();
            }

            #region IComparable<DispatchTable<TInvoker>.LongKey4> Members

            public int CompareTo(LongKey4 other)
            {
                int result = _arg1Type.CompareTo(other._arg1Type);

                if (result != 0)
                    return result;

                result = _arg2Type.CompareTo(other._arg2Type);

                if (result != 0)
                    return result;

                result = _arg3Type.CompareTo(other._arg3Type);

                if (result != 0)
                    return result;

                return _arg4Type.CompareTo(other._arg4Type);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0} [{1}; {2}; {3}; {4}]", GetType(), _arg1Type, _arg2Type, _arg3Type, _arg4Type);
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Two methods has the same signature
    /// </summary>
    public class DispatchTableConflictException : Exception
    {
        /// <summary>
        /// Newly added method. Its addition raised this exception.
        /// </summary>
        public MethodInfo AddingMethod;

        public Type[] AddingMethodTypes;

        /// <summary>
        /// Method already existing in dispatch table.
        /// </summary>
        public MethodInfo ConflictingMethod;

        public Type[] ConflictingMethodTypes;

        /// <summary>
        /// Key in dispatch table for both methods (adding and already existing)
        /// </summary>
        public object DispatchTableKey;

        public ServiceDispatcher Dispatcher;

        public DispatchTableConflictException(object dispatchTableKey, string message)
            : base(message)
        {
            DispatchTableKey = dispatchTableKey;
        }
    }
}