using System;
using System.Linq;
using System.Text;

namespace Amnesia.Domain.Model
{
    public class Hash
    {
        public Hash(byte[] bytes)
        {
            Bytes = bytes;
        }

        public Hash(string hex)
        {
            Bytes = StringToByteArray(hex);
        }

        public byte[] Bytes { get; }

        public override string ToString()
        {
            return ByteArrayToString(Bytes);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                   .Where(x => x % 2 == 0)
                   .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                   .ToArray();
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static implicit operator Hash(byte[] bytes)
        {
            return new Hash(bytes);
        }

        public static implicit operator byte[](Hash hash)
        {
            return hash.Bytes;
        }
    }
}
