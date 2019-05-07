using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Amnesia.Domain.Context;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Seed
{
    public class Seed
    {
        private readonly BlockchainContext context;

        public Seed(BlockchainContext context)
        {
            this.context = context;
        }

        public void SeedData()
        {
            var block = new Block
            {
                PreviousBlockHash = Hash.StringToByteArray("0"),
                Nonce = 0,
                ContentHash = Hash.StringToByteArray(""),
                Content = new Content(),
                PreviousBlock = null
            };

            var content = new Content
            {
                
            };
            
            var data = new Data
            {
                PreviousDefinitionHash = Hash.StringToByteArray("prev"),
                Signature = Hash.StringToByteArray("signature"),
                Blob = Hash.StringToByteArray("Hallo dit is wat data.")
            };

            var definition = new Definition
            {
                
            };
                
            var state = new State
            {
                
            };
                
            context.Database.EnsureCreated();

            var b = context.Blocks.FirstOrDefault();
            if (b == null)
            {
                context.Blocks.Add(block);
            }

            var c = context.Contents.FirstOrDefault();
            if (c == null)
            {
                context.Contents.Add(content);
            }

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

            var s = context.State.FirstOrDefault();
            if (s == null)
            {
                context.State.Add(state);
            }

            context.SaveChanges();
        }
    }
    
    
}