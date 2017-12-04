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
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(Index + PreviousHash + Timestamp + Data)).ToHexString();
            }
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

            var hash = newBlock.CalculateHash();
            if (hash != newBlock.Hash)
            {
                Console.WriteLine("invalid hash: " + hash + " " + newBlock.Hash);
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + Index;
                hash = hash * 23 + PreviousHash?.GetHashCode() ?? 0;
                hash = hash * 23 + Timestamp.GetHashCode();
                hash = hash * 23 + Data?.GetHashCode() ?? 0;
                hash = hash * 23 + Hash?.GetHashCode() ?? 0;

                return hash;
            }
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
