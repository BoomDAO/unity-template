namespace ItsJackAnton.Utility
{
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    public static class HashUtil
    {

        private const uint FNV_offset_basis32 = 2166136261;
        private const uint FNV_prime32 = 16777619;
        private const ulong FNV_offset_basis64 = 14695981039346656037;
        private const ulong FNV_prime64 = 1099511628211;



        #region ID & HASH
        public static string GenID()
        {
            return System.Guid.NewGuid().ToString("D");
        }
        public static int ToHash(this string value)
        {
            return Animator.StringToHash(value);
        }
        public static int GenHash()
        {
            return GenID().ToHash();
        }
        #endregion

        /// <summary>
        /// non cryptographic stable hash code,
        /// it will always return the same hash for the same
        /// string.
        ///
        /// This is simply an implementation of FNV-1 32 bit xor folded to 16 bit
        /// </summary>
        /// <returns>The stable hash32.</returns>
        /// <param name="txt">Text.</param>
        public static ushort ToHash16(this string txt)
        {
            uint hash32 = txt.ToHash32();

            return (ushort)((hash32 >> 16) ^ hash32);
        }


        /// <summary>
        /// non cryptographic stable hash code,
        /// it will always return the same hash for the same
        /// string.
        ///
        /// This is simply an implementation of FNV-1 32 bit
        /// </summary>
        /// <returns>The stable hash32.</returns>
        /// <param name="txt">Text.</param>
        public static uint ToHash32(this string txt)
        {
            unchecked
            {
                uint hash = FNV_offset_basis32;
                for (int i = 0; i < txt.Length; i++)
                {
                    uint ch = txt[i];
                    hash = hash * FNV_prime32;
                    hash = hash ^ ch;
                }
                return hash;
            }
        }

        /// <summary>
        /// non cryptographic stable hash code,
        /// it will always return the same hash for the same
        /// string.
        ///
        /// This is simply an implementation of FNV-1  64 bit
        /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
        /// </summary>
        /// <returns>The stable hash32.</returns>
        /// <param name="txt">Text.</param>
        internal static ulong ToHash64(this string txt)
        {
            unchecked
            {
                ulong hash = FNV_offset_basis64;
                for (int i = 0; i < txt.Length; i++)
                {
                    ulong ch = txt[i];
                    hash = hash * FNV_prime64;
                    hash = hash ^ ch;
                }
                return hash;
            }
        }


        #region Sha256
        public static string Sha256_Hash(this string value)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                for (int i = 0; i < result.Length; i++)
                {
                    Sb.Append(result[i].ToString("x2"));
                }
            }

            return Sb.ToString();
        }
        #endregion
    }
}