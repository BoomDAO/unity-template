namespace ItsJackAnton.Values
{
    using UnityEngine;

    public struct UOption<T>
    {
        public bool HasValue { get; private set; }
        private T value;
        public T Value { get { return value; } set { this.value = value; HasValue = true; } }

        public UOption(T value) : this()
        {
            Value = value;
        }
    }
}