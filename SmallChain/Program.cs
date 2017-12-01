using System.Threading.Tasks;

namespace SmallChain
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAll().Wait();
        }

        public static async Task RunAll()
        {
            var node1 = new ClientNode("node1");
            var node2 = new ClientNode("node2");
            var node3 = new ClientNode("node3");
            var node4 = new ClientNode("node4");
            var node5 = new ClientNode("node5");
            var node6 = new ClientNode("node6");

            node1.CreateGenesisBlock();

            node1.Connect(node2);
            node2.Connect(node3);
            node3.Connect(node4);
            node4.Connect(node5);
            node5.Connect(node6);

            node1.Connect(node3);
            node2.Connect(node4);
            node3.Connect(node5);
            node4.Connect(node6);

            await Task.WhenAll(
                node1.Run(logging: true),
                node2.Run(),
                node3.Run(),
                node4.Run(),
                node5.Run(),
                node6.Run());
        }
    }
}
