using System;
using System.Security.Cryptography;
using System.Text;

namespace SmallChain
{
    public class Block : IEquatable<Block>
    {
        public static readonly Block Genesis = new Block
        {
            Index = 0,
            PreviousHash = "0",
            Timestamp = 1512035841,
            Data = "genesis",
            Hash = "c5cd6055557f80eaf2ab3809d1e544d0158198acdd16b80bfa259bfaf521399f"
        };
        
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public long Timestamp { get; set; }
        public string Data { get; set; }
        public string Hash { get; set; }

        public void AppendHash()
        {
            Hash = CalculateHash(Index, PreviousHash, Timestamp, Data);
        }

        public bool Validate(Block newBlock)
        {
            if (newBlock == null)
            {
                Console.WriteLine("block is null");
                return false;
            }

            if (Index + 1 != newBlock.Index)
            {
                Console.WriteLine("invalid index");
                return false;
            }

            if (Hash != newBlock.PreviousHash)
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

        public static string CalculateHash(long index, string previous, long timestamp, string data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(index + previous + timestamp + data)).ToHexString();
            }
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Block);
        }

        public bool Equals(Block other)
        {
            if (other == null)
            {
                return false;
            }

            return Index == other.Index
                && PreviousHash == other.PreviousHash
                && Timestamp == other.Timestamp
                && Data == other.Data
                && Hash == other.Hash;
        }

        public override string ToString()
        {
            return $"Index: {Index}, Hash: {Hash}, Data: {Data}";
        }
    }
}
