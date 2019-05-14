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

        public byte[] HashObject()
        {
            return PrimaryHash.Hash;
        }

        public abstract CompositeHash PrimaryHash { get; }
    }
}