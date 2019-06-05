using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amnesia.Application.Validation.Context;
using Amnesia.Domain.Context;
using Amnesia.Domain.ViewModels;
using Newtonsoft.Json;

namespace Amnesia.Application.Services
{
    public class SeedService
    {
        private readonly BlockchainContext context;
        private readonly StateService stateService;
        private readonly BlockchainService blockchain;

        private const string Filename = "Amnesia.Application.SeedData.json";

        public SeedService(BlockchainContext context, StateService stateService, BlockchainService blockchain)
        {
            this.context = context;
            this.stateService = stateService;
            this.blockchain = blockchain;
        }

        public void SeedData()
        {
            context.Database.EnsureCreated();

            using var stream = typeof(SeedService).Assembly.GetManifestResourceStream(Filename);
            using var streamReader = new StreamReader(stream);
            var json = streamReader.ReadToEnd();

            var lists = JsonConvert.DeserializeObject<BlockchainLists>(json);

            var memoryContext = new MemoryValidationContext();

            foreach (var block in lists.Blocks)
            {
                memoryContext.AddBlock(block.ToBlock());
            }

            foreach (var content in lists.Contents)
            {
                memoryContext.AddContent(content.ToContent());
            }

            foreach (var definition in lists.Definitions)
            {
                memoryContext.AddDefinition(definition.ToDefinition());
            }

            foreach (var data in lists.Data)
            {
                memoryContext.AddData(data.ToData());
            }
            
            blockchain.SaveContext(memoryContext);
            stateService.ChangeState(lists.Blocks.Last().ToBlock().Hash);
        }

        public void Dump()
        {
            context.Data.RemoveRange(context.Data);
            context.Definitions.RemoveRange(context.Definitions);
            context.Contents.RemoveRange(context.Contents);
            context.Blocks.RemoveRange(context.Blocks);
            context.State.RemoveRange(context.State);
        }

        private class BlockchainLists
        {
            public IList<BlockViewModel> Blocks { get; set; }
            public IList<ContentViewModel> Contents { get; set; }
            public IList<DefinitionViewModel> Definitions { get; set; }
            public IList<DataViewModel> Data { get; set; }
        }
    }
}