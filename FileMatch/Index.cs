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
        public List<Entry> Entries { get; private set; }
        public List<Bucket> Buckets { get; }

        public Index(IGraphNetwork graphNetwork, Node node) : base(node, char.MaxValue, char.MinValue)
        {
            _graphNetwork = graphNetwork;
            Entries = new List<Entry>();
            Buckets = new List<Bucket>();
        }

        public async Task SelectParent()
        {
            var nodes = await _graphNetwork.Scan();

            Parent = new Node(-1, "");

            if (!nodes.Any())
            {
                return;
            }

            Parent = nodes.OrderBy(n => n.Depth).First();
            Depth = Parent.Depth + 1;
            PropertyChange(nameof(Parent));

            await _graphNetwork.Join(Parent, this);
        }

        internal Index(IGraphNetwork graphNetwork, Node node, int rangeStart, int rangeEnd, List<Entry> entries) : base(node, char.MinValue, char.MaxValue)
        {
            _graphNetwork = graphNetwork;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
            Entries = entries;
            Buckets = new List<Bucket>();
        }

        public async Task Insert(Entry entry, Node source = null)
        {
            if (source == null)
            {
                entry.Locations.Add(this);
            }

            if (Parent.Depth == -1 || source.Address == Parent.Address) 
            {
                await Insert(entry);
            }

            if (!IsInRange(entry))
            {
                await _graphNetwork.Insert(Parent, this, entry);
                return;
            }

            await Insert(entry);
        }

        private async Task<bool> Insert(Entry entry)
        {
            var key = entry.Key;

            foreach (var child in Buckets)
            {
                if (key > child.RangeStart)
                {
                    await _graphNetwork.Insert(child, this, entry);
                    child.RangeStart = Math.Min(RangeStart, key);
                    child.RangeEnd = Math.Max(RangeEnd, key);
                    return true;
                }
            }

            Entries.Add(entry);
            RangeStart = Math.Min(RangeStart, key);
            RangeEnd = Math.Max(RangeEnd, key);
            return true;
        }

        public async Task<IEnumerable<Entry>> Search(EntryName entryName)
        {
            if (!IsInRange(entryName))
            {
                if (Parent.Depth != -1)
                    return await _graphNetwork.Search(Parent, entryName.Name);

                return new List<Entry>();
            }

            foreach (var child in Buckets)
            {
                if (child.IsInRange(entryName))
                {
                    return await _graphNetwork.Search(child, entryName.Name);
                }
            }

            return Entries.Where(t => t.Name.IsMatch(entryName));
        }

        public async Task<bool> Join(Node node)
        {
            var keys = Entries.Select(d => d.Key).DefaultIfEmpty();
            var start = (keys.Max() - keys.Min()) / (Buckets.Count + 1) + keys.Max();
            Buckets.Insert(0, new Bucket(node, start, start));
            await _graphNetwork.Reset(node, start, start);

            return true;
        }

        public async Task<bool> Reset(char start, char end)
        {
            RangeStart = start;
            RangeEnd = end;

            var entries = Entries.Select(x => x);
            Entries.Clear();

            foreach (var key in entries)
            {
                await Insert(key, this);
            }

            return true;
        }
    }
}
