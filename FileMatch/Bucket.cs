namespace FileMatch
{
    public class Bucket : Node
    {
        public int RangeStart { get; protected set; }
        public int RangeEnd { get; }

        public Bucket(Node node, int rangeStart, int rangeEnd) : base(node.Depth, node.Address)
        {
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
        }
    }
}
