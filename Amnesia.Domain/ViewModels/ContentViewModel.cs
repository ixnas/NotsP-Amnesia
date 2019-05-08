using System.Collections.Generic;
using System.Linq;
using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class ContentViewModel
    {
        public string Hash { get; set; }
        public List<string> Definitions { get; set; }
        public List<string> Mutations { get; set; }

        public ContentViewModel(Content content)
        {
            
            Hash = Model.Hash.ByteArrayToString(content.Hash);
            Definitions = MapHashes(content.Definitions);
            Mutations = MapHashes(content.Mutations);
        }

        private List<string> MapHashes(IList<Definition> list)
        {
            return list.Select(item => Model.Hash.ByteArrayToString(item.DataHash)).ToList();
        }
    }
}