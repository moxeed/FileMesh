namespace FileMatch
{
    public class Node
    {
        public int Depth { get; set; }
        public string Address { get; set; }

        public Node(int depth, string address)
        {
            Depth = depth;
            Address = address;
        }
    }
}
