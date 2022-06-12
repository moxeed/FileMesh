using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMatch
{
    public class Index : Bucket
    {
        const int HealthDelay = 3000;
        private readonly IGraphNetwork _graphNetwork;

        public Node Parent { get; private set; }
        public List<Entry> Entries { get; private set; }
        public List<Bucket> Buckets { get; private set; }

        public Index(IGraphNetwork graphNetwork, Node node) : base(node)
        {
            _graphNetwork = graphNetwork;
            Entries = new List<Entry>();
            Buckets = new List<Bucket>();
        }

        public async Task SelectParent()
        {
            Parent = new Node(-1, "");

            var nodes = await _graphNetwork.Scan();

            if (!nodes.Any())
            {
                return;
            }

            Parent = nodes.OrderBy(n => n.Depth).First();
            Depth = Parent.Depth + 1;
            PropertyChange(nameof(Parent));

            await _graphNetwork.Join(Parent, this);
        }

        public async Task Heart()
        {
            while (true)
            {
                await Task.Delay(HealthDelay);
                try
                {
                    if (Parent.Depth != -1)
                        await _graphNetwork.Health(Parent, this);
                }
                catch
                {
                    await SelectParent();
                }

                Buckets = Buckets.Where(b => !b.IsDead).ToList();

                foreach (var bucket in Buckets)
                {
                    bucket.Age();
                }
            }
        }

        public async Task<bool> Insert(Entry entry, Node source = null)
        {
            if (source == null)
            {
                entry.Locations.Add(this);
            }

            if (Parent.Depth == -1 || source?.Address == Parent.Address)
            {
                await Insert(entry);
                return true;
            }

            if (!IsInRange(entry))
            {
                await _graphNetwork.Insert(Parent, this, entry);
                return true;
            }

            return await Insert(entry);
        }

        private async Task<bool> Insert(Entry entry)
        {
            var max = Similarity(entry.Name) - CharSet.Length;
            Bucket bestBucket = this;

            foreach (var child in Buckets)
            {
                var similarity = child.Similarity(entry.Name) - child.CharSet.Length;
                if (similarity > max)
                {
                    max = similarity;
                    bestBucket = child;
                }
            }

            if (bestBucket == this)
            {
                Entries.Add(entry);
            }
            else
            {
                await _graphNetwork.Insert(bestBucket, this, entry);
            }

            bestBucket.CharSet = string.Concat((bestBucket.CharSet + entry.Name.ToLower()).Distinct());
            return true;
        }

        public bool Health(Node child)
        {
            foreach (var bucket in Buckets)
            {
                if (bucket.Address == child.Address)
                {
                    bucket.Live();
                    break;
                }
            }

            return true;
        }

        public async Task<IEnumerable<Entry>> Search(string name)
        {
            foreach (var child in Buckets)
            {
                if (child.IsInRange(name))
                {
                    return await _graphNetwork.Search(child, name);
                }
            }

            if (!IsInRange(name))
            {
                if (Parent.Depth != -1)
                    return await _graphNetwork.Search(Parent, name);

                return new List<Entry>();
            }

            return Entries.Where(t => t.IsMatch(name));
        }

        public async Task<bool> Join(Node node)
        {
            var bucket = new Bucket(node)
            {
                CharSet = await _graphNetwork.Reset(node)
            };

            Buckets.Insert(0, bucket);
            return true;
        }
        public string Reset()
        {
            return CharSet;
        }
    }
}
