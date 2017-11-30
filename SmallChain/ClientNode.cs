using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmallChain
{
    public class ClientNode
    {
        private BlockChain localChain = new BlockChain();
        private List<ClientNode> connectedNodes = new List<ClientNode>();
        private readonly string name;
        private int counter;

        public ClientNode(string name)
        {
            this.name = name;
        }

        public void OnReceiveChain(BlockChain remoteChain)
        {
            if (!remoteChain.Validate())
            {
                Console.WriteLine("invalid chain received");
                return;
            }

            if (localChain.GetLength() > remoteChain.GetLength())
            {
                Console.WriteLine("smaller length chain received");
                return;
            }

            localChain = remoteChain;
        }

        public void Connect(ClientNode node)
        {
            connectedNodes.Add(node);
        }

        public void AddNewBlock()
        {
            counter++;
            if (localChain.TryAddBlock(name + "-" + counter))
            {
                Console.WriteLine(name + " added block " + localChain.GetLatestBlock());
            }
        }

        public async Task Run(bool logging = false)
        {
            var random = new Random();

            while (true)
            {
                foreach (var node in connectedNodes)
                {
                    node.OnReceiveChain(localChain);
                }

                await Task.Delay(1000);

                var i = random.Next(1, 6);
                if (i == 5)
                {
                    AddNewBlock();

                    if (logging)
                    {
                        Console.WriteLine(localChain);
                    }
                }
            }
        }
    }
}
