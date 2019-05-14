using System;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Amnesia.Domain.ViewModels;

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

        public Task Start(Block payload)
        {
            cancellationTokenSource = new CancellationTokenSource();
            
            return Task.Run(() =>
            {
                Mine(payload);
            }, cancellationTokenSource.Token);
        }

        private void Mine(Block payload)
        {
            var hash = payload.Hash;

            while (!CheckHash(hash))
            {
                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                payload.Nonce++;
                hash = payload.HashObject();
            }

            Console.WriteLine("Nonce: {0}", payload.Nonce);
            Console.WriteLine("Hash: {0}", Hash.ByteArrayToString(hash));
                
            Mined?.Invoke(payload);
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
    }
}