namespace Boom.Patterns.Broadcasts
{
    using Boom.Utility;
    using Boom.Values;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

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

            private class BroadcastGroup
            {
                private InitValue<IBroadcastState> msg;
                private readonly LinkedList<DelegateInfo> listeners;
                public bool IsInit { get { return msg.IsInit; } }

                public BroadcastGroup()
                {
                    this.listeners = new();
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
                        else if (force)
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
            }

            private readonly string typeName;
            private Dictionary<string, BroadcastGroup> broadcastGroups;

            public string TypeName { get { return typeName; } }

            public BroadcastInfo(string typeName)
            {
                this.typeName = typeName;
                broadcastGroups = new();
            }

            public void Register<T>(Action<T> listener, bool invokeOnRegistration = false, string tag = "main") where T : IBroadcastState, new()
            {
                if(!broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    broadcastGroup = new();
                    broadcastGroups.Add(tag, broadcastGroup);
                }

                broadcastGroup.Register<T>(listener, invokeOnRegistration);
            }
            public void Unregister(int handlerHashCode, string tag)
            {
                if (broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    broadcastGroup.Unregister(handlerHashCode);
                }
            }

            public bool Invoke(IBroadcastState msg, bool force, string tag)
            {
                if (!broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    broadcastGroup = new();
                    broadcastGroups.Add(tag, broadcastGroup);
                }

                return broadcastGroup.Invoke(msg, force);
            }

            public T Read<T>(string tag) where T : IBroadcastState, new()
            {
                if (!broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    return new();
                }

                return broadcastGroup.Read<T>();
            }

            public bool IsInit(string tag)
            {
                if (!broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    return new();
                }

                return broadcastGroup.IsInit;
            }

            public bool TryDispose<T>(out T outVal, string tag) where T : IBroadcastState, new()
            {
                outVal = default;
                if (!broadcastGroups.TryGetValue(tag, out BroadcastGroup broadcastGroup))
                {
                    return false;
                }

                outVal = broadcastGroup.Read<T>();

                broadcastGroups.Remove(tag);

                return true;
            }

            public void TryDisposeAll(params string[] tagsToIgnore)
            {
                if(broadcastGroups.Count > 0)
                {
                    var keys = broadcastGroups.Keys.ToList();

                    foreach( var k in keys)
                    {
                        if (!tagsToIgnore.Has(e => e == k))
                        {
                            broadcastGroups.Remove(k);
                        }
                    }
                }
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

        public static void Register<T>(Action<T> listener, bool invokeOnRegistration = false, string tag = "main") where T : IBroadcastState, new()
        {
            string typeName = typeof(T).FullName;
            ushort key = HashUtil.ToHash16(typeName);

            if (events.TryGetValue(key, out BroadcastInfo broadcast))
            {
                if (broadcast.TypeName == typeName) broadcast.Register(listener, invokeOnRegistration, tag);
                else $"> Broadcast: There was conflict with two events type {typeName} trying to register on {broadcast.TypeName} because both generate same hash".Warning(nameof(BroadcastState));
            }
            else
            {
                broadcast = new BroadcastInfo(typeName);
                broadcast.Register(listener, invokeOnRegistration, tag);
                events.Add(key, broadcast);
            }
        }
        public static void Unregister<T>(Action<T> listener, string tag = "main") where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                int handlerHashCode = listener.GetHashCode();
                targets.Unregister(handlerHashCode, tag);
            }
        }

        public static bool TryRead<T>(out T value, string tag = "main") where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                value = targets.Read<T>(tag);
                return true;
            }
            else
            {
                value = new();
                return false;
            }
        }
        public static bool WasInit<T>(string tag = "main") where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets)) return targets.IsInit(tag);
            else return false;
        }

        public static bool Invoke<T>(T msg, bool force = false, string tag = "main") where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);
            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                return targets.Invoke(msg, force, tag);
            }

            targets = new BroadcastInfo(typeof(T).FullName);
            targets.Invoke(msg, force, tag);
            events.Add(key, targets);
            return false;
        }
        public static bool Invoke<T>(Func<T, T> refactor, bool force = false, string tag = "main") where T : IBroadcastState, new()
        {
            if(TryRead(out T msg, tag))
            {
                msg = refactor(msg);
                return Invoke(msg, force, tag);
            }
            return Invoke(refactor(new()), force, tag);
        }
        public static bool ForceInvoke<T>(T msg, string tag = "main") where T : IBroadcastState, new()
        {
            return Invoke(msg, true, tag);
        }
        public static bool ForceInvoke<T>(Func<T, T> refactor, string tag = "main") where T : IBroadcastState, new()
        {
            return Invoke(refactor, true, tag);
        }

        public static bool TryDispose<T>(out T outVal, string tag = "main") where T : IBroadcastState, new()
        {
            outVal = default;

            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                targets.TryDispose(out outVal, tag);

                return true;
            }
            else return false;
        }

        public static bool TryDisposeAll<T>(params string[] tagsToIgnore) where T : IBroadcastState, new()
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                targets.TryDisposeAll(tagsToIgnore);

                return true;
            }
            else return false;
        }
    }
}