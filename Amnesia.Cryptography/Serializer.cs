using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Amnesia.Cryptography
{
    public class Serializer
    {
        /// <summary>
        /// Serialize object to a byte array.
        /// </summary>
        public byte[] Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserialize byte array to an object.
        /// </summary>
        public object Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(ms);
            }
        }
    }
}