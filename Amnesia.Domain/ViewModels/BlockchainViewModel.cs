using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.ViewModels
{
    public class BlockchainViewModel
    {
        public string Current { get; set; }
        public List<BlockViewModel> Blocks { get; set; }

        public BlockchainViewModel(List<Block> blocks)
        {
            Current = Hash.ByteArrayToString(blocks.First().Hash);
            MapBlocksToViewModel(blocks);
        }

        private void MapBlocksToViewModel(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                Blocks.Add(BlockViewModel.FromBlock(block));
            }
        }
    }
}