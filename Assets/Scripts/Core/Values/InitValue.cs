namespace ItsJackAnton.Values
{
    using System;
    using UnityEngine;
    [Serializable]
    public struct InitValue<T>
    {
        [SerializeField] T value;

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (IsInit == false) InitialValue = value;
                this.value = value;
                IsInit = true;
            }
        }

        [field: SerializeField, ShowOnly] public bool IsInit { get; private set; }
        [field: SerializeField, ShowOnly] public T InitialValue { get; private set; }
    }
}