using System;
using System.Collections.Generic;
using System.Linq;

namespace Amnesia.Application.Helper
{
    public class ByteArrayEqualityComparer : EqualityComparer<byte[]>
    {
        private const int BytesToHash = 3;

        public override bool Equals(byte[] x, byte[] y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.SequenceEqual(y);
        }

        public override int GetHashCode(byte[] obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.Take(BytesToHash).Sum(b => b);
        }
    }
}