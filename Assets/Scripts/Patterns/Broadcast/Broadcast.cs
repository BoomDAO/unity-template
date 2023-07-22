namespace Boom.Patterns.Broadcasts
{
    using Boom.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public static class Broadcast
    {
        public delegate void BroadcastDelegate(IBroadcast broadcast);
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
            private readonly LinkedList<DelegateInfo> listeners;
            public string TypeName { get { return typeName; } }

            public BroadcastInfo(string typeName)
            {
                this.typeName = typeName;
                listeners = new();
            }

            public void Register<T>(Action<T> listener)
            {
                int handlerHashCode = listener.GetHashCode();
                listeners.AddFirst(new DelegateInfo(handlerHashCode, CreateBroadcastDelegate(listener)));
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

            public void Invoke(IBroadcast msg)
            {
                LinkedListNode<DelegateInfo> runner = listeners.First;

                while (runner != null)
                {
                    runner.Value.reference.Invoke(msg);
                    runner = runner.Next;
                }
            }

            private static BroadcastDelegate CreateBroadcastDelegate<T>(Action<T> handler)
            {
                void LogicContainer(IBroadcast _broadcast)
                {
                    handler?.Invoke((T)_broadcast);
                }
                return LogicContainer;
            }
        }
        private static readonly Dictionary<ushort, BroadcastInfo> events = new();

        public static void Register<T>(Action<T> listener) where T : struct, IBroadcast
        {
            string typeName = typeof(T).FullName;
            ushort key = HashUtil.ToHash16(typeName);

            if (events.TryGetValue(key, out BroadcastInfo broadcast))
            {
                if (broadcast.TypeName == typeName) broadcast.Register(listener);
                else $"> Broadcast: There was conflict with two events type {typeName} trying to register on {broadcast.TypeName} because both generate same hash".Warning(nameof(Broadcast));
            }
            else
            {
                var newBroadcast = new BroadcastInfo(typeName);
                newBroadcast.Register(listener);
                events.Add(key, newBroadcast);
            }
        }
        public static void Unregister<T>(Action<T> listener) where T : struct, IBroadcast
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);

            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                int handlerHashCode = listener.GetHashCode();
                targets.Unregister(handlerHashCode);
            }
        }

        public static void Invoke<T>() where T : struct, IBroadcast
        {
             Invoke(new T());
        }
        public static void Invoke<T>(T msg) where T : struct, IBroadcast
        {
            ushort key = HashUtil.ToHash16(typeof(T).FullName);
            if (events.TryGetValue(key, out BroadcastInfo targets))
            {
                targets.Invoke(msg);
            }
        }
    }
}