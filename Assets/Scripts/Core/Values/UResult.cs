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
}