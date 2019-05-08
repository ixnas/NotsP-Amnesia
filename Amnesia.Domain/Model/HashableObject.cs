using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using PeterO.Cbor;

namespace Amnesia.Domain.Model
{
    [Serializable]
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
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(ToByteArray());
        }

        public byte[] ToByteArray()
        {
            var properties = GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(IncludeInHashAttribute)) != null)
                .ToList();

            var obj = CBORObject.NewMap();

            if (!properties.Any())
            {
                throw new Exception($"Type {GetType()} has no properties with attribute IncludeInHash");
            }

            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(this);

                obj.Add(name, value);
            }

            return obj.EncodeToBytes();
        }
    }
}