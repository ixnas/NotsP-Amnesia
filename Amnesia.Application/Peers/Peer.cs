namespace Amnesia.Application.Peers
{
    public class Peer
    {
        public Peer(string url, string key)
        {
            Url = url;
            Key = key;
        }

        public string Url { get; }
        public string Key { get; }
    }
}