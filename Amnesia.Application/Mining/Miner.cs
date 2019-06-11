using System;
using System.Collections;
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
            
            await Task.Run(() =>
            {
                Mine(payload);
            });
        }

        private void Mine(Block payload)
        {
            var hash = payload.HashObject();
            
            while (!CheckHash(hash, difficulty))
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                payload.Nonce++;
                hash = payload.HashObject();
            }

            payload.Hash = hash;

            Console.WriteLine("Nonce: {0}", payload.Nonce);
            Console.WriteLine("Hash: {0}", Hash.ByteArrayToString(payload.Hash));
            
            Mined?.Invoke(payload);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public static bool CheckHash(byte[] hash, int difficulty)
        {
            var bitArray = new BitArray(hash);

            for (var i = 0; i < difficulty; i++)
            {
                if (bitArray[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}