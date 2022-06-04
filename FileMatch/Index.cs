using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMatch
{
    public class Index : Bucket
    {
        private readonly IGraphNetwork _graphNetwork;

        public Node Parent { get; private set; }
        public ICollection<Entry> Entries { get; private set; }
        public ICollection<Bucket> Buckets { get; }

        public Index(IGraphNetwork graphNetwork, Node node): base(node, char.MinValue, char.MaxValue)
        {
            _graphNetwork = graphNetwork;
            Entries = new HashSet<Entry>();
            Buckets = new HashSet<Bucket>();
        }

        public void SelectParent()
        {
            var nodes = _graphNetwork.Scan().Result;
            Parent = nodes.OrderBy(n => n.Depth).FirstOrDefault() ?? new Node(-1, "");
        }

        internal Index(IGraphNetwork graphNetwork, Node node, ICollection<Entry> entries) : base(node, char.MinValue, char.MaxValue)
        {
            _graphNetwork = graphNetwork;
            Entries = entries;
            Buckets = new HashSet<Bucket>();
        }

        public Task Insert(Entry entry)
        {
            var relatedNode = GetRalatedNode(entry.Key);

            if (relatedNode != null) {
                _graphNetwork.Insert(relatedNode, entry);
            }

            Entries.Add(entry);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Entry>> Search(EntryName entryName)
        {
            var relatedNode = GetRalatedNode(entryName.Key);

            if (relatedNode != null)
            {
                _graphNetwork.Search(relatedNode, entryName.Name);
            }

            return Task.FromResult(Entries.Where(t => t.Name.IsMatch(entryName)));
        }

        public Index Split(Node newChild)
        {
            var mid = (RangeStart + RangeEnd) / 2;
            var outEntries = Entries.Where(e => e.Key <= mid).ToList();

            RangeStart = mid;
            Entries = Entries.Where(e => e.Key > mid).ToList();

            return new Index(_graphNetwork, newChild, outEntries);
        }

        private Node GetRalatedNode(int key) {

            if (key > RangeEnd || key < RangeStart)
            {
                return Parent;
            }

            foreach (var bucket in Buckets)
            {
                if (key <= RangeEnd || key >= RangeStart)
                {
                    return bucket;
                }
            }

            return null;
        }
    }
}
