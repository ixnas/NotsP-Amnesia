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
        public DataViewModel Data { get; set; }

        public DefinitionViewModel(Definition definition)
        {
            Hash = Model.Hash.ByteArrayToString(definition.Hash);
            DataHash = Model.Hash.ByteArrayToString(definition.DataHash);
            Meta = definition.Meta;
            PreviousDefinition = (definition.PreviousDefinitionHash != null) ? Model.Hash.ByteArrayToString(definition.PreviousDefinitionHash) : null;
            Signature = Model.Hash.ByteArrayToString(definition.Signature);
            Data = (definition.Data != null) ? new DataViewModel(definition.Data) : null;
        }
    }
}