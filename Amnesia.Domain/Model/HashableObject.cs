using System;
using System.Security.Cryptography;
using System.Text;
using Amnesia.Cryptography;

namespace Amnesia.Domain.Model
{
    [Serializable]
    public abstract class HashableObject
    {
        public byte[] Hash { get; set; }
        
        protected byte[] CalculateSha256Hash()
        {
            var serializer = new Serializer();
            
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                byte[] bytes = sha256Hash.ComputeHash(serializer.Serialize(this));
                return bytes;
            }  
        }
        
        public string Calculate256HashString()
        {
            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < Hash.Length; i++)  
            {  
                builder.Append(Hash[i].ToString("x2"));  
            }  
            return builder.ToString();
        }
    }
}