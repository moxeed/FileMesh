using FileMatch;
using Rssdp;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    internal class GraphNetwork : IGraphNetwork
    {
        public async Task<IEnumerable<Node>> Scan()
        {
            using (var deviceLocator = new SsdpDeviceLocator())
            {
                var foundDevices = await deviceLocator.SearchAsync($"urn:{nameof(Service)}:device:{nameof(Node)}:1");

                return foundDevices.Select(d =>
                {
                    var uri = d.DescriptionLocation.ToString().Replace("/", "");
                    var data = uri.Split(':');
                    return new Node(int.Parse(data[2]), data[1]);
                }).Where(n => n.Address != Http.GetIp());
            }
        }

        public Task Insert(Node node, Node source, Entry entry) => Http.Post<bool>(node, nameof(Insert), new InsertModel
        {
            Source = source,
            Entry = entry
        });

        public async Task<IEnumerable<Entry>> Search(Node node, string term) => await Http.Get<List<Entry>>(node, nameof(Search) + "?term=" + term);

        public Task Reset(Node node, int start, int end) => Http.Get<bool>(node, nameof(Reset) + $"?start={start}&end={end}");

        public Task Join(Node parent, Node node) => Http.Post<bool>(parent, nameof(Join), node);
    }
}
