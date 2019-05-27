using System.Collections.Generic;
using Amnesia.Domain.Entity;

namespace Amnesia.Domain.ViewModels
{
    public class DefinitionViewModel
    {
        public string Hash { get; set; }
        public string DataHash { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public string PreviousDefinition { get; set; }
        public string Signature { get; set; }
        public string Key { get; set; }

        public static DefinitionViewModel FromDefinition(Definition definition)
        {
            var vm = new DefinitionViewModel
            {
                Hash = Model.Hash.ByteArrayToString(definition.Hash),
                DataHash = Model.Hash.ByteArrayToString(definition.DataHash),
                Meta = definition.Meta,
                PreviousDefinition = definition.PreviousDefinitionHash == null
                                     ? null
                                     : Model.Hash.ByteArrayToString(definition.PreviousDefinitionHash),
                Signature = Model.Hash.ByteArrayToString(definition.Signature),
                Key = definition.Key
            };
            return vm;
        }
    }
}