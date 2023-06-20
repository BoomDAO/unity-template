namespace ItsJackAnton.Patterns.Broadcasts
{
    using ItsJackAnton.Utility;
    using System.Collections.Generic;
    using UnityEngine.Events;

    public class UnityBroadcast
    {
        private class EventData
        {
            private readonly UnityEvent listeners;
            public int ListenerCount { get; private set; }

            public EventData(UnityAction listener)
            {
                listeners = new UnityEvent();
                Register(listener);
            }


            public void Invoke()
            {
                listeners.Invoke();
            }

            public int Register(UnityAction listener)
            {
                listeners.AddListener(listener);
                ++ListenerCount;
                return ListenerCount;
            }
            public int Unregister(UnityAction listener)
            {
                listeners.RemoveListener(listener);
                --ListenerCount;
                return ListenerCount;
            }
        }

        private readonly Dictionary<int, EventData> channels = new();

        public int Register(UnityAction listener, int channel = 0)
        {
            int key = channel;

            if (channels.TryGetValue(key, out EventData listeners))
            {
                return listeners.Register(listener);
            }
            else
            {
                listeners = new EventData(listener);
                channels.Add(key, listeners);
                return 1;
            }
        }
        public int Unregister(UnityAction listener, int channel = 0)
        {
            int key = channel;

            if (channels.TryGetValue(key, out EventData listeners))
            {
                return listeners.Unregister(listener);
            }
            return 0;
        }
        public void Invoke(int channel = 0)
        {
            int key = channel;

            if (channels.TryGetValue(key, out EventData listeners))
            {
                listeners.Invoke();
            }
        }

        public void Dispose()
        {
            channels.Clear();
        }

        public int GetListenersCount(int channel)
        {
            if (channels.TryGetValue(channel, out EventData targets)) return targets.ListenerCount;
            else return 0;
        }
    }
    public class UnityBroadcast<T>
    {
        private class EventData<K>
        {
            private readonly UnityEvent<K> listeners;
            public int ListenerCount { get; private set; }

            public EventData(UnityAction<K> listener)
            {
                listeners = new UnityEvent<K>();
                Register(listener);
            }

            public void Invoke(K param)
            {
                listeners.Invoke(param);
            }

            public int Register(UnityAction<K> listener)
            {
                listeners.AddListener(listener);
                ++ListenerCount;
                return ListenerCount;
            }
            public int Unregister(UnityAction<K> listener)
            {
                listeners.RemoveListener(listener);
                --ListenerCount;
                return ListenerCount;
            }
        }

        private readonly Dictionary<int, EventData<T>> channels = new();

        public int Register(UnityAction<T> listener, int channel = 0)
        {

            int key = channel;

            if (channels.TryGetValue(key, out EventData<T> listeners))
            {
                return listeners.Register(listener);
            }
            else
            {
                listeners = new EventData<T>(listener);
                channels.Add(key, listeners);
                return 1;
            }
        }
        public int Unregister(UnityAction<T> listener, int channel = 0)
        {
            int key = channel;

            if (channels.TryGetValue(key, out EventData<T> listeners))
            {
                return listeners.Unregister(listener);
            }
            return 0;
        }
        public void Invoke(T msg, int channel = 0)
        {
            int key = channel;

            if (channels.TryGetValue(key, out EventData<T> listeners))
            {
                listeners.Invoke(msg);
            }
        }

        public void Dispose()
        {
            channels.Clear();
        }
        public int GetListenersCount(int channel)
        {
            if (channels.TryGetValue(channel, out EventData<T> targets)) return targets.ListenerCount;
            else return 0;
        }
    }
}