namespace ItsJackAnton.Values
{
    using System;

    [Serializable]
    public struct KeyValue<Key, Val>
    {
        public Key key;
        public Val value;
        public KeyValue(Key key, Val value) { this.key = key; this.value = value; }
    }
    [Serializable]
    public class KeyValuePointer<Key, Val>
    {
        public Key key;
        public Val value;
        public KeyValuePointer(Key key, Val value) { this.key = key; this.value = value; }
    }
}