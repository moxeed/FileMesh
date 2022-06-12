using System.Linq;

namespace FileMatch
{
    public class Bucket : Node
    {
        const byte MaxAge = 3;

        public byte _age;
        string _charSet;
        public string CharSet
        {
            get
            {
                return _charSet;
            }
            set
            {
                _charSet = value;
                PropertyChange(nameof(CharSet));
            }
        }

        public Bucket(Node node) : base(node.Depth, node.Address)
        {
            CharSet = string.Empty;
        }

        public bool IsInRange(Entry entry) => entry.Name.All(c => _charSet.Contains(c));
        public bool IsInRange(string name) => name.All(c => _charSet.Contains(c));
        public int Similarity(string term) => CharSet.Intersect(term).Count();

        internal void Age() => _age++;
        internal void Live()
        {
            if (_age > 0)
            {
                _age--;
            }
        }
        internal bool IsDead => _age > MaxAge;
    }
}
