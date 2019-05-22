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
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public DataViewModel Data { get; set; }

        public DefinitionViewModel(Definition definition)
        {
            Hash = Model.Hash.ByteArrayToString(definition.Hash);
            DataHash = Model.Hash.ByteArrayToString(definition.DataHash);
            Meta = definition.Meta;
            Signature = definition.Signature;
            Key = definition.Key;
            Data = new DataViewModel(definition.Data);

            if (definition.PreviousDefinitionHash != null)
            {
                PreviousDefinition = Model.Hash.ByteArrayToString(definition.PreviousDefinitionHash);
            }
        }
    }
}