namespace ItsJackAnton.Patterns.Broadcasts
{
    using ItsJackAnton.Utility;
    using ItsJackAnton.Values;
    using System;
    using System.Collections.Generic;
    public static class BroadcastState
    {
        private delegate void BroadcastDelegate(IBroadcastState broadcast);
        private class BroadcastInfo
        {
            private class DelegateInfo
            {
                public int hash;
                public BroadcastDelegate reference;
                //Construct
                public DelegateInfo(int hash, BroadcastDelegate reference)
                {
                    this.hash = hash;
                    this.reference = reference;
                }
            }

            private readonly string typeName;
            private InitValue<IBroadcastState> msg;
            private readonly LinkedList<DelegateInfo> listeners;

            public string TypeName { get { return typeName; } }
            public bool IsInit { get { return msg.IsInit; } }

            public BroadcastInfo(string typeName)
            {
                this.typeName = typeName;
                listeners = new();
            }

            public void Register<T>(Action<T> listener, bool invokeOnRegistration = false) where T : IBroadcastState, new()
            {
                int handlerHashCode = listener.GetHashCode();
                listeners.AddFirst(new DelegateInfo(handlerHashCode, CreateBroadcastDelegate(listener)));

                if (invokeOnRegistration)
                {
                    if (msg.IsInit)
                    {
                        listener.Invoke((T)msg.Value);
                    }
                    else
                    {
                        listener.Invoke(new());
                    }
                }
            }
            public void Unregister(int handlerHashCode)
            {
                LinkedListNode<DelegateInfo> runner = listeners.First;

                while (runner != null)
                {
                    if (runner.Value.hash == handlerHashCode)
                    {
                        listeners.Remove(runner);
                        return;
                    }
                    runner = runner.Next;
                }
            }

            public bool Invoke(IBroadcastState msg, bool force)
            {
                void _Invoke()
                {
                    this.msg.Value = msg;

                    LinkedListNode<DelegateInfo> runner = listeners.First;

                    while (runner != null)
                    {
                        runner.Value.reference.Invoke(this.msg.Value);
                        runner = runner.Next;
                    }
                }

                if (this.msg.IsInit)
                {
                    if (!this.msg.Value.Equals(msg))
                    {
                        _Invoke();
                        return true;
                    }
                    else if(force)
                    {
                        _Invoke();
                        return true;
                    }
                    else return false;
                }
                else
                {
                    _Invoke();
                    return true;
                }
            }

            public T Read<T>() where T : IBroadcastState, new()
            {
                if (msg.IsInit) return (T)msg.Value;
                return new();
            }

            private static BroadcastDelegate CreateBroadcastDelegate<T>(Action<T> handler)
            {
                void LogicContainer(IBroadcastState broadcast)
                {
                    handler?.Invoke((T)broadcast);
                }
                return LogicContainer;
            }
        }
        private static readonly Dictionary<ushort, BroadcastInfo> events = new();

        public static void Register<T>(Action<T> listener, bool invokeOnRegistration = false) where T : IBroadcastState, new()
        {
            string typeName = typeof(T).FullName;
            ushort key = HashUtil.ToHash16(typeName);

            if (events.TryGetValue(key, out BroadcastInfo broadcast))
            {
                if (broadcast.TypeName == typeName) broadcast.Register(listener, invokeOnRegistration);
                else $"> Broadcast: There was conflict with two events type {typeName} trying to register on {broadcast.TypeName} because both generate same hash".Warning(nameof(BroadcastState));
            }
            else
            {
                broadcast = new BroadcastInfo(typeName);
                broadcast.Register(listener, invokeOnRegistration);
                events.Add(key, broadcast);
            }
        }
        public static void Unregister<T>(Action<T> listener) where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                int handlerHashCode = listener.GetHashCode();
                targets.Unregister(handlerHashCode);
            }
        }

        public static bool TryRead<T>(out T value) where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                value = targets.Read<T>();
                return true;
            }
            else
            {
                value = new();
                return false;
            }
        }
        public static bool WasInit<T>() where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets)) return targets.IsInit;
            else return false;
        }

        public static bool Invoke<T>(T msg, bool force = false) where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);
            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                return targets.Invoke(msg, force);
            }

            targets = new BroadcastInfo(typeof(T).FullName);
            targets.Invoke(msg, force);
            events.Add(key, targets);
            return false;
        }
        public static bool Invoke<T>(Func<T, T> refactor, bool force = false) where T : IBroadcastState, new()
        {
            if(TryRead(out T msg))
            {
                msg = refactor(msg);
                return Invoke(msg, force);
            }
            return Invoke(refactor(new()), force);
        }
        public static bool ForceInvoke<T>(T msg) where T : IBroadcastState, new()
        {
            return Invoke(msg, true);
        }
        public static bool ForceInvoke<T>(Func<T, T> refactor) where T : IBroadcastState, new()
        {
            return Invoke(refactor, true);
        }
    }
}