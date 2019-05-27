using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Cryptography;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Services
{
    public class SeedService
    {
        private readonly BlockchainContext context;

        public SeedService(BlockchainContext context)
        {
            this.context = context;
        }

        //TODO: Add correct signatures
        public void SeedData()
        {
            context.Database.EnsureCreated();
            
            var signature = new CompositeHash(Encoding.ASCII.GetBytes("Handtekening")).Hash;
            var keys = new KeyPair(2048);
            
            var data = new Data
            {
                PreviousDefinitionHash = null,
                Signature = signature,
                Blob = Encoding.ASCII.GetBytes("Dit is test data.")
            };
            
            var definition = new Definition
            {
                DataHash = data.Hash,
                PreviousDefinitionHash = null,
                Signature = signature,
                Key = null,
                IsMutation = false,
                Meta = null,
                Data = data,
                PreviousDefinition = null
            };
            var definitions = new List<byte[]> {definition.Hash};
            var mutations = new List<byte[]>();
            var content = new Content
            {
                Definitions = definitions,
                Mutations = mutations,
                Block = null
            };
            var block = new Block
            {
                PreviousBlockHash = null,
                Nonce = 0,
                Content = content,
                ContentHash = content.Hash,
                PreviousBlock = null
            };
            var state = new State
            {
                PeerId = "peer1",
                CurrentBlock = block,
                CurrentBlockHash = block.Hash
            };
            
            var d = context.Data.FirstOrDefault();
            if (d == null)
            {
                context.Data.Add(data);
            }
            var de = context.Definitions.FirstOrDefault();
            if (de == null)
            {
                context.Definitions.Add(definition);
            }
            var c = context.Contents.FirstOrDefault();
            if (c == null)
            {
                context.Contents.Add(content);
            }
            var b = context.Blocks.FirstOrDefault();
            if (b == null)
            {
                context.Blocks.Add(block);
            }
            var s = context.State.FirstOrDefault();
            if (s == null)
            {
                context.State.Add(state);
            }
            
            context.SaveChanges();
        }
        
    }
}