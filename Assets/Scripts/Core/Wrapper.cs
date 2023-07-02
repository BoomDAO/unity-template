namespace ItsJackAnton.Values
{
    using System;

    [Serializable]
    public class Wrapper<T>
    {
        public T value;

        public Wrapper(T val)
        {
            this.value = val;
        }

        public virtual T GetValue()
        {
            return value;
        }
    }
}
