using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PeterO.Cbor;

namespace Amnesia.Domain.Model
{
    public class CompositeHash
    {
        private readonly object obj;
        private readonly IList<string> properties = new List<string>();
        private readonly IList<Action<CBORObject>> customProperties = new List<Action<CBORObject>>();

        public CompositeHash(object obj)
        {
            this.obj = obj;
        }

        public CompositeHash Add(string propertyName)
        {
            properties.Add(propertyName);

            return this;
        }

        public CompositeHash Add(Action<CBORObject> adder)
        {
            customProperties.Add(adder);

            return this;
        }

        public byte[] Hash => HashBytes(EncodeToBytes());

        private static byte[] HashBytes(byte[] bytes)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(bytes);
        }

        public byte[] EncodeToBytes()
        {
            var cbor = CBORObject.NewMap();

            foreach (var name in properties)
            {
                var prop = obj.GetType().GetProperty(name);
                var value = prop.GetValue(obj);

                cbor.Add(name, value);
            }

            foreach (var adder in customProperties)
            {
                adder(cbor);
            }

            return cbor.EncodeToBytes();
        }
    }
}