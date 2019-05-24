using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amnesia.WebApi.Models
{
    public class JSONDefinition
    {
        public string Hash;
        public string PreviousDefinitionHash;
        public string Signature;
        public JSONData Data;
        public JSONMeta Meta;
    }
}
