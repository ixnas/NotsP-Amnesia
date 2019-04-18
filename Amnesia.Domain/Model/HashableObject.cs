using System;

namespace Amnesia.Domain.Model
{
    public abstract class HashableObject
    {
        private Lazy<byte[]> hash;

        public byte[] Hash
        {
            get => hash.Value;
            set => hash = new Lazy<byte[]>(value);
        }

        protected HashableObject()
        {
            hash = new Lazy<byte[]>(HashObject);
        }

        protected byte[] HashObject()
        {
            // TODO implement real hash coding
            var bytes = BitConverter.GetBytes(GetHashCode());

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}