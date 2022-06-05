using FileMatch;
using Rssdp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMesh.Infrastructure
{
    internal class GraphNetwork : IGraphNetwork
    {
        public async Task<IEnumerable<Node>> Scan()
        {
            using (var deviceLocator = new SsdpDeviceLocator())
            {
                var foundDevices = await deviceLocator.SearchAsync($"urn:{nameof(FileMesh)}:device:{nameof(Node)}:1");

                return foundDevices.Select(d =>
                {
                    var uri = d.DescriptionLocation.ToString().Replace("/", "");
                    var data = uri.Split(':');
                    return new Node(int.Parse(data[2]), data[1]);
                }).Where(n => n.Address != Http.GetIp());
            }
        }

        public Task Insert(Node node, Entry entry) => Http.Post<bool>(node, nameof(Insert), entry);
        public Task<IEnumerable<Entry>> Search(Node node, string term) => Http.Get<IEnumerable<Entry>>(node, nameof(Search) + "?term=" + term);
        public Task<Index> Split(Node parent, Node newChild) => Http.Post<Index>(parent, nameof(Split), newChild);
    }
}
