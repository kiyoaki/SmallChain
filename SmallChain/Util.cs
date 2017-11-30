using System;
using System.Security.Cryptography;
using System.Text;

namespace SmallChain
{
    public static class Util
    {
        public static Block CreateGenesisBlock()
        {
            using (var sha256 = SHA256.Create())
            {
                return new Block
                {
                    Index = 0,
                    PreviousHash = "0",
                    Timestamp = 1512035841,
                    Data = "genesis",
                    Hash = "c5cd6055557f80eaf2ab3809d1e544d0158198acdd16b80bfa259bfaf521399f"
                };
            }
        }

        public static string CalculateHash(long index, string previous, long timestamp, string data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(index + previous + timestamp + data)).ToHexString();
            }
        }

        public static bool Validate(this Block previousBlock, Block newBlock)
        {
            if (previousBlock == null || newBlock == null)
            {
                Console.WriteLine("block is null");
                return false;
            }

            if (previousBlock.Index + 1 != newBlock.Index)
            {
                Console.WriteLine("invalid index");
                return false;
            }

            if (previousBlock.Hash != newBlock.PreviousHash)
            {
                Console.WriteLine("invalid previous hash");
                return false;
            }

            var hash = CalculateHash(newBlock.Index, newBlock.PreviousHash, newBlock.Timestamp, newBlock.Data);
            if (hash != newBlock.Hash)
            {
                Console.WriteLine("invalid hash: " + hash + " " + newBlock.Hash);
                return false;
            }

            return true;
        }
    }
}
