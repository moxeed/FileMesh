namespace FileMatch
{
    public class Bucket : Node
    {
        int _rangeStart;
        public int RangeStart
        {
            get
            {
                return _rangeStart;
            }
            set
            {
                _rangeStart = value;
                PropertyChange(nameof(RangeStart));
            }
        }

        int _rangeEnd;
        public int RangeEnd
        {
            get
            {
                return _rangeEnd;
            }
            set
            {
                _rangeEnd = value;
                PropertyChange(nameof(RangeEnd));
            }
        }

        public Bucket(Node node, int rangeStart, int rangeEnd) : base(node.Depth, node.Address)
        {
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
        }

        public bool IsInRange(Entry entry) => entry.Key >= RangeStart && entry.Key <= RangeEnd;
        public bool IsInRange(EntryName entry) => entry.Key >= RangeStart && entry.Key <= RangeEnd;
    }
}
