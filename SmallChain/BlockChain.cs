using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallChain
{
    public class BlockChain
    {
        private readonly ReaderWriterLockSlim chainsLock = new ReaderWriterLockSlim();
        private readonly List<Block> chain = new List<Block>();

        public Block GetLatestBlock()
        {
            chainsLock.EnterReadLock();
            try
            {
                return chain[chain.Count - 1];
            }
            finally
            {
                chainsLock.ExitReadLock();
            }
        }

        public int GetLength()
        {
            chainsLock.EnterReadLock();
            try
            {
                return chain.Count;
            }
            finally
            {
                chainsLock.ExitReadLock();
            }
        }

        public void CreateGenesis()
        {
            chainsLock.EnterWriteLock();
            try
            {
                chain.Add(Block.Genesis);
            }
            finally
            {
                chainsLock.ExitWriteLock();
            }
        }

        public bool TryAddBlock(string data = null)
        {
            chainsLock.EnterUpgradeableReadLock();
            try
            {
                if (chain.Count == 0)
                {
                    return false;
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
                    chainsLock.EnterWriteLock();
                    try
                    {
                        chain.Add(nextBlock);
                    }
                    finally
                    {
                        chainsLock.ExitWriteLock();
                    }
                    return true;
                }
            }
            finally
            {
                chainsLock.ExitUpgradeableReadLock();
            }
            return false;
        }

        public bool Validate()
        {
            chainsLock.EnterReadLock();
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
                chainsLock.ExitReadLock();
            }
            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("===========================");

            chainsLock.EnterReadLock();
            try
            {
                foreach (var block in chain)
                {
                    builder.AppendLine(block.ToString());
                }
            }
            finally
            {
                chainsLock.ExitReadLock();
            }

            builder.AppendLine("===========================");
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
