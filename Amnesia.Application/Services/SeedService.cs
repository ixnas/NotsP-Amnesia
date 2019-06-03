using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Amnesia.Application.Mining;
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
        
        public async Task SeedData()
        {
            context.Database.EnsureCreated();
            
            var keys = new KeyPair(2048);
            var data = MakeData(keys);
            var definition = MakeDefinition(keys, data);
            var content = MakeContent(definition);
            var block = await MakeBlock(content);
            
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

        private Data MakeData(KeyPair keys)
        {
            var blob = Encoding.UTF8.GetBytes("Dit is test data.");
            var signature = keys.PrivateKey.SignData(blob);
            
            return new Data
            {
                PreviousDefinitionHash = null,
                Signature = signature,
                Key = keys.PublicKey.ToPEMString(),
                Blob = blob
            };
        }

        private Definition MakeDefinition(KeyPair keys, Data data)
        {
            var definition = new Definition
            {
                DataHash = data.Hash,
                PreviousDefinitionHash = null,
                Key = keys.PublicKey.ToPEMString(),
                IsMutation = false,
                IsMutable = true,
                Data = data,
                PreviousDefinition = null
            };
            var signature = keys.PrivateKey.SignData(definition.SignatureHash.Hash);
            definition.Signature = signature;
            return definition;
        }

        private Content MakeContent(Definition definition)
        {
            var definitions = new List<byte[]> {definition.Hash};
            var mutations = new List<byte[]>();
            return new Content
            {
                Definitions = definitions,
                Mutations = mutations
            };
        }

        private async Task<Block> MakeBlock(Content content)
        {
            var block = new Block
            {
                PreviousBlockHash = null,
                Nonce = 0,
                Content = content,
                ContentHash = content.Hash,
                PreviousBlock = null
            };
            var miner = new Miner(20);
            miner.Mined += b => { block = b; };
            await miner.Start(block);
            return block;
        }
    }
}