using System;
using System.Collections.Generic;

namespace Amnesia.Application.Peers
{
    public class PeerConfiguration
    {
        public string NetworkId { get; set; }
        public IDictionary<string, string> Peers { get; set; } = new Dictionary<string, string>();

        public ApiConfiguration Api { get; set; } = new ApiConfiguration();

        public void Validate()
        {
            if (string.IsNullOrEmpty(NetworkId))
                throw new ArgumentException("NetworkId cannot be empty");

            foreach (var (id, url) in Peers)
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentException("Peer id may not be empty");

                var uri = new Uri(url, UriKind.Absolute);

                if (uri.Scheme != "http" &&
                    uri.Scheme != "https")
                    throw new ArgumentException("Can only use http or https");

                if (string.IsNullOrEmpty(uri.Host))
                    throw new ArgumentException("A hostname is required");
            }
        }
    }

    public class ApiConfiguration
    {
        public string SendBlock { get; set; } = "/blocks?peer={0}";
        public string Blocks { get; set; } = "/blocks/";
        public string Contents { get; set; } = "/blocks/{0}/content";
        public string Definitions { get; set; } = "/definitions/{0}";
        public string Data { get; set; } = "/definitions/{0}/data";
        public string Keys { get; set; } = "/keys/%s/definitions?depth=%i";
    }
}