using System;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;

namespace Amnesia.Application.Mining
{
    public class Miner
    {
        private readonly int difficulty;
        public CancellationTokenSource cancellationTokenSource { get; private set; }

        public event Action<Block> Mined;

        public Miner(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public async Task Start(Block payload)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            await Task.Run(() =>
            {
                var hash = payload.Hash;

                while (!CheckHash(hash))
                {
                    payload.Nonce++;
                    hash = payload.HashObject();

                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }

                Console.WriteLine("Nonce: {0}", payload.Nonce);
                Console.WriteLine("Hash: {0}", Sha256HashToString(hash));
                
                Mined?.Invoke(payload);
            }, cancellationTokenSource.Token);
            
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        private bool CheckHash(byte[] hash)
        {
            var bigInteger = new BigInteger(hash);
            return (bigInteger & ((1 << difficulty) - 1)) == 0;
        }
        
        private string Sha256HashToString(byte[] sha256)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < sha256.Length; i++)
            {
                builder.Append(sha256[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}