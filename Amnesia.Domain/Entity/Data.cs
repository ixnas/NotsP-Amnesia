using System;
using System.Text;
using System.Text.RegularExpressions;
using Amnesia.Domain.Model;

namespace Amnesia.Domain.Entity
{
    public class Data : HashableObject
    {
        public byte[] PreviousDefinitionHash { get; set; }
        public byte[] Signature { get; set; }
        public string Key { get; set; }
        public byte[] Blob { get; set; }

        public override CompositeHash PrimaryHash => new CompositeHash(this)
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(Blob))
            .Add(nameof(Signature))
            .Add(nameof(Key));

        public CompositeHash SignatureHash => new CompositeHash(this)
            .Add(nameof(PreviousDefinitionHash))
            .Add(nameof(Blob));

        public byte[] ParseMutationHash()
        {
            try
            {
                var str = Encoding.UTF8.GetString(Blob);

                var regex = new Regex("DELETE ([0-9a-fA-F]+)");
                var match = regex.Match(str);

                if (!match.Success)
                {
                    return null;
                }

                var hash = match.Groups[1].Value;

                return Model.Hash.StringToByteArray(hash);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
