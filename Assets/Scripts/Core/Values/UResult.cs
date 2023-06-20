namespace ItsJackAnton.Values
{
    public enum UResultTag
    {
        Ok, Err, None
    }

    public class UResult<OK,ERR>
    {
        private object value;

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
        public OK AsOk()
        {
            return (OK)value;
        }
        public ERR AsErr()
        {
            return (ERR)value;
        }
        public UResult<OK, ERR> Ok(object value)
        {
            this.value = value;
            Tag = UResultTag.Ok;
            return this;
        }
        public UResult<OK, ERR> Err(object value)
        {
            this.value = value;
            Tag = UResultTag.Err;
            return this;
        }
    }
}