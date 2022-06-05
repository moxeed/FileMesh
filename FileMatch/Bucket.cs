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
            protected set
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
            protected set
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
    }
}
