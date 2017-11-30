using System;

namespace SmallChain
{
    public class Block : IEquatable<Block>
    {
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public long Timestamp { get; set; }
        public string Data { get; set; }
        public string Hash { get; set; }

        public void AppendHash()
        {
            Hash = Util.CalculateHash(Index, PreviousHash, Timestamp, Data);
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

            var hash = Util.CalculateHash(newBlock.Index, newBlock.PreviousHash, newBlock.Timestamp, newBlock.Data);
            if (hash != newBlock.Hash)
            {
                Console.WriteLine("invalid hash: " + hash + " " + newBlock.Hash);
                return false;
            }

            return true;
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
