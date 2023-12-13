using Boom.Utility;
using EdjCase.ICP.Candid.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class AccountIdentifierUtil
{
    public delegate string AccountIdentifier(string t, List<byte> sa);

    public static List<byte> SUBACCOUNT_ZERO = new List<byte>
    {
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };

    private static List<byte> ads = new List<byte>
    {
        10, 97, 99, 99, 111, 117, 110, 116, 45, 105, 100
    };

    //public static string FromText(string data, List<byte> sa = null)
    //{
    //    var p = Principal.FromText(data);
    //    return FromPrincipal(p, sa);
    //}
    //public static string FromPrincipal(Principal data, List<byte> sa = null)
    //{

    //}
    //public static string FromBlob(Principal data, List<byte> sa = null)
    //{

    //}

    public static string FromBytes(byte[] data, List<byte> sa = null)
    {
        if (sa == null)
        {
            sa = new List<byte>(SUBACCOUNT_ZERO);
        }

        //var combined = sa.CreateDeepCopy();
        //combined.Add(data);

        List<byte> combined = new(ads);
        combined.AddRange(data);
        combined.AddRange(sa);

        List<byte> hash = SHA224Hash(combined);
        List<byte> crc = CRC32Hash(hash);

        List<byte> result = new List<byte>(crc);
        result.AddRange(hash);

        return HexEncode(result);
    }

    public static bool Equal(string a, string b)
    {
        return string.Equals(a, b);
    }

    public static int Hash(string text)
    {
        return text.GetHashCode();
    }

    private static List<byte> SHA224Hash(List<byte> data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] result = sha256.ComputeHash(data.ToArray());
            return new List<byte>(result);
        }
    }

    private static List<byte> CRC32Hash(List<byte> data)
    {
        using (CRC32 crc32 = new CRC32())
        {
            byte[] result = crc32.ComputeHash(data.ToArray());
            return new List<byte>(result);
        }
    }

    private static string HexEncode(List<byte> data)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in data)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}

public class CRC32 : HashAlgorithm
{
    public const uint DefaultPolynomial = 0xedb88320u;
    public const uint DefaultSeed = 0xffffffffu;

    private static uint[] defaultTable;
    private static uint seed;
    private static uint[] crc32Table;
    private uint hash;

    public CRC32()
    {
        crc32Table = InitializeTable(DefaultPolynomial);
        seed = DefaultSeed;
        Initialize();
    }

    public CRC32(uint polynomial, uint seed)
    {
        crc32Table = InitializeTable(polynomial);
        CRC32.seed = seed;
        Initialize();
    }

    public override void Initialize()
    {
        hash = seed;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        hash = CalculateHash(crc32Table, hash, array, ibStart, cbSize);
    }

    protected override byte[] HashFinal()
    {
        byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
        HashValue = hashBuffer;
        return hashBuffer;
    }

    private uint[] InitializeTable(uint polynomial)
    {
        if (polynomial == DefaultPolynomial && defaultTable != null)
            return defaultTable;

        uint[] createTable = new uint[256];
        for (int i = 0; i < 256; i++)
        {
            uint entry = (uint)i;
            for (int j = 0; j < 8; j++)
                if ((entry & 1) == 1)
                    entry = (entry >> 1) ^ polynomial;
                else
                    entry = entry >> 1;
            createTable[i] = entry;
        }

        if (polynomial == DefaultPolynomial)
            defaultTable = createTable;

        return createTable;
    }

    private static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size)
    {
        uint crc = seed;
        for (int i = start; i < size - 1; i++)
            crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
        return crc;
    }

    private byte[] UInt32ToBigEndianBytes(uint x)
    {
        return new byte[] {
            (byte)((x >> 24) & 0xff),
            (byte)((x >> 16) & 0xff),
            (byte)((x >> 8) & 0xff),
            (byte)(x & 0xff)
        };
    }
}
