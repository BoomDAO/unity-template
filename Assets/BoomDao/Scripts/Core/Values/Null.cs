namespace Boom.Values
{

    using System;

    public class Null : IEquatable<object>
    {
        public static bool operator ==(Null a, object b)
        {
            return null == b;
        }
        public static bool operator !=(Null a, object b)
        {
            return null != b;
        }
        public override bool Equals(object other)
        {
            return other == null;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}