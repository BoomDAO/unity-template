namespace Boom.Values
{
    public enum UResultTag
    {
        Ok, Err, None
    }

    public class UResult<OK,ERR>
    {
        public object Value { get; private set; }

        public UResult()
        {
        }
        public UResult(OK value)
        {
            Ok(value);
        }
        public UResult(ERR value)
        {
            Err(value);
        }

        public UResultTag Tag { get; private set; } = UResultTag.None;

        public bool IsOk
        {
            get { return Tag == UResultTag.Ok; }
        }
        public bool IsErr
        {
            get { return Tag == UResultTag.Err; }
        }

        public OK AsOk()
        {
            return (OK)Value;
        }
        public OK AsOkorDefault(OK defaultVal = default)
        {
            if (Tag != UResultTag.Ok) return defaultVal;
            return (OK)Value;
        }
        public ERR AsErr()
        {
            return (ERR)Value;
        }

        public UResult<OK, ERR> Ok(object value)
        {
            this.Value = value;
            Tag = UResultTag.Ok;
            return this;
        }
        public UResult<OK, ERR> Err(object value)
        {
            this.Value = value;
            Tag = UResultTag.Err;
            return this;
        }
    }

    public class UResult<T> : UResult<UResult<T>.OK<T>, UResult<T>.ERR<T>>
    {
        public abstract class Base<K>
        {
            public K Value { private set; get; }

            protected Base(K message)
            {
                Value = message;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class OK<K> : Base<K>
        {
            public OK(K message) : base(message)
            {
            }
        }
        public class ERR<K> : Base<K>
        {
            public ERR(K message) : base(message)
            {

            }
        }

        public UResult(OK<T> value)
        {
            Ok(value);
        }
        public UResult(ERR<T> value)
        {
            Err(value);
        }

        public override string ToString()
        {
            if (Tag == UResultTag.Ok) return AsOk().ToString();
            else if (Tag == UResultTag.Ok) return AsErr().ToString();

            return null;
        }

        public static implicit operator string(UResult<T> a)
        {
            return a.ToString();
        }
    }
}