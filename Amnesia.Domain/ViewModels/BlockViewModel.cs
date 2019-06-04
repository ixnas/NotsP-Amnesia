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
        public BlockViewModel() {}
        public static BlockViewModel FromBlock(Block block)
        {
            var vm = new BlockViewModel
            {
                Hash = Model.Hash.ByteArrayToString(block.Hash),
                Previous = block.PreviousBlockHash == null
                    ? null
                    : Model.Hash.ByteArrayToString(block.PreviousBlockHash),
                Content = Model.Hash.ByteArrayToString(block.ContentHash),
                Nonce = block.Nonce
            };
            return vm;
        }

        public Block ToBlock()
        {
            return new Block
            {
                Hash = Model.Hash.StringToByteArray(Hash),
                ContentHash = Model.Hash.StringToByteArray(Content),
                PreviousBlockHash = Previous == null ? null : Model.Hash.StringToByteArray(Previous),
                Nonce = Nonce
            };
        }
    }
}