using FileMatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMesh.Infrastructure
{
    internal class GraphNetwork : IGraphNetwork
    {
        public async Task<IEnumerable<Node>> Scan()
        {
            var ip = Http.GetIp();
            var network = ip.Substring(0, ip.LastIndexOf('.'));
            var nodes = new List<Node>();
            foreach (var i in Enumerable.Range(1, 2))
            {
                try
                {
                    var node = await Http.Get<Node>($"{network}.{i}");
                    nodes.Add(node);
                }
                catch(Exception e) {}
            }

            return nodes;
        }

        public Task Insert(Node node, Entry entry) => Http.Post<bool>(node, nameof(Insert), entry);
        public Task<IEnumerable<Entry>> Search(Node node, string term) => Http.Get<IEnumerable<Entry>>(node, nameof(Search) + "?term=" + term);
        public Task<Index> Split(Node parent, Node newChild) => Http.Post<Index>(parent, nameof(Split), newChild);
    }
}
