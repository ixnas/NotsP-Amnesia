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

        public ContentViewModel(){}
        public static ContentViewModel FromContent(Content content)
        {
            var vm = new ContentViewModel
            {
                Hash = Model.Hash.ByteArrayToString(content.Hash),
                Definitions = content.Definitions == null
                              ? null
                              : MapHashes(content.Definitions),
                Mutations = content.Mutations == null 
                              ? null
                              :MapHashes(content.Mutations)
            };
            return vm;
        }

        private static List<string> MapHashes(IEnumerable<byte[]> list)
        {
            return list.Select(Model.Hash.ByteArrayToString).ToList();
        }
    }
}