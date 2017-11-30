using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallChain
{
    public class BlockChain
    {
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
        private readonly List<Block> chain = new List<Block>();

        public Block GetLatestBlock()
        {
            Lock.EnterReadLock();
            try
            {
                return chain[chain.Count - 1];
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public int GetLength()
        {
            Lock.EnterReadLock();
            try
            {
                return chain.Count;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public bool TryAddBlock(string data = null)
        {
            Lock.EnterUpgradeableReadLock();
            try
            {
                if (chain.Count == 0)
                {
                    Lock.EnterWriteLock();
                    try
                    {
                        chain.Add(Block.Genesis);
                    }
                    finally
                    {
                        Lock.ExitWriteLock();
                    }
                    return true;
                }

                var previousBlock = GetLatestBlock();
                var nextBlock = new Block
                {
                    Index = previousBlock.Index + 1,
                    PreviousHash = previousBlock.Hash,
                    Timestamp = DateTime.UtcNow.ToUnixTime(),
                    Data = data
                };
                nextBlock.AppendHash();
                if (previousBlock.Validate(nextBlock))
                {
                    Lock.EnterWriteLock();
                    try
                    {
                        chain.Add(nextBlock);
                    }
                    finally
                    {
                        Lock.ExitWriteLock();
                    }
                    return true;
                }
            }
            finally
            {
                Lock.ExitUpgradeableReadLock();
            }
            return false;
        }

        public bool Validate()
        {
            Lock.EnterReadLock();
            try
            {
                if (chain.Count == 0)
                {
                    return false;
                }

                if (!Block.Genesis.Equals(chain[0]))
                {
                    return false;
                }

                if (chain.Count == 1)
                {
                    return true;
                }

                for (var i = 1; i < chain.Count - 1; i++)
                {
                    var block = chain[i];
                    if (block == null)
                    {
                        return false;
                    }

                    if (chain[i].PreviousHash != chain[i - 1].Hash)
                    {
                        return false;
                    }
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("===========================");

            Lock.EnterReadLock();
            try
            {
                foreach (var block in chain)
                {
                    builder.AppendLine(block.ToString());
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }

            builder.AppendLine("===========================");
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
