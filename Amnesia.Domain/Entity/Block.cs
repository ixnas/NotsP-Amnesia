using System.Security.Cryptography;
using System.Text;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Block : HashableObject
    {
        public byte[] PreviousBlockHash { get; set; }
        public int Nonce { get; set; }
        public byte[] ContentHash { get; set; }

        public Content Content { get; set; }
        public Block PreviousBlock { get; set; }

        public void Mine(int difficulty, Block block)
        {
            var hash = Block.ComputeSha256Hash(block);
           
        }
        
        static string ComputeSha256Hash(byte[] data)  
        {  
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(data);  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        }
    }
}
