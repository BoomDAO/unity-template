namespace Boom.Values
{
    using UnityEngine;

    public class UOption<T>
    {
        public bool HasValue { get; private set; }
        private T value;
        public T Value { get { return value; } set { this.value = value; HasValue = true; } }
        public UOption() { }

        public UOption(T value)
        {
            HasValue = true;
            Value = value;
        }
    }
}