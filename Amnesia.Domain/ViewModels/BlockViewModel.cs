using System.Collections.Generic;
using System.Net;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.ViewModels
{
    public class BlockViewModel
    {
        public string Hash { get; set; }
        public string Previous { get; set; }
        public string Content { get; set; }
        public int Nonce { get; set; }
        public BlockViewModel(Block block)
        {    
            Hash = Model.Hash.ByteArrayToString(block.Hash);
            Previous = Model.Hash.ByteArrayToString(block.PreviousBlockHash);
            Content = Model.Hash.ByteArrayToString(block.ContentHash);
            Nonce = block.Nonce;
        }

        public BlockViewModel(List<Block> blocks)
        {
            
        }
    }
}