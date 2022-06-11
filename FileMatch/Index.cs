using FileMatch.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMatch
{
    public class Index : Bucket
    {
        private readonly IGraphNetwork _graphNetwork;

        public Node Parent { get; private set; }
        public List<Entry> Entries { get; private set; }
        public ICollection<Bucket> Buckets { get; }

        public Index(IGraphNetwork graphNetwork, Node node): base(node, char.MinValue, char.MaxValue)
        {
            _graphNetwork = graphNetwork;
            Entries = new List<Entry>();
            Buckets = new HashSet<Bucket>();
        }

        public async Task SelectParent()
        {
            var nodes = await _graphNetwork.Scan();

            Parent = new Node(-1, "");

            if (!nodes.Any()) {
                return;
            }

            Parent = nodes.OrderBy(n => n.Depth).First();
            Depth = Parent.Depth + 1;

            var index = await _graphNetwork.Split(Parent, this);
            Entries.AddRange(index.Entries);
            RangeStart = index.RangeStart;
            RangeEnd = index.RangeEnd;
        }

        internal Index(IGraphNetwork graphNetwork, Node node, int rangeStart, int rangeEnd, List<Entry> entries) : base(node, char.MinValue, char.MaxValue)
        {
            _graphNetwork = graphNetwork;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
            Entries = entries;
            Buckets = new HashSet<Bucket>();
        }

        public Task Insert(Entry entry)
        {
            entry.Locations.Add(this);

            var relatedNode = GetRalatedNode(entry.Key);

            if (relatedNode != null) {
                return _graphNetwork.Insert(relatedNode, entry);
            }

            Entries.Add(entry);
            return Task.CompletedTask;
        }

        public Task PostInsert(Entry entry)
        {
            var relatedNode = GetRalatedNode(entry.Key);

            if (relatedNode != null)
            {
                return _graphNetwork.Insert(relatedNode, entry);
            }

            Entries.Add(entry);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Entry>> Search(EntryName entryName)
        {
            var relatedNode = GetRalatedNode(entryName.Key);

            if (relatedNode != null)
            {
                return _graphNetwork.Search(relatedNode, entryName.Name);
            }
            return Task.FromResult(Entries.Where(t => t.Name.IsMatch(entryName)));
        }

        public IndexModel Split(Node newChild)
        {
            var mid = (Buckets.Max(b => b.RangeEnd) + RangeEnd) / 2;
            var outEntries = Entries.Where(e => e.Key <= mid).ToList();
            var index = new IndexModel {
                RangeEnd = mid,
                RangeStart = RangeStart,
                Entries = outEntries
            };

            Entries = Entries.Where(e => e.Key > mid).ToList();
            Buckets.Add(new Bucket(newChild, RangeStart, mid));
            return index;
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
