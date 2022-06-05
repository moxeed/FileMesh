using System.ComponentModel;

namespace FileMatch
{
    public class Bucket : Node, INotifyPropertyChanged
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RangeStart)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RangeEnd)));
            }
        }

        public Bucket(Node node, int rangeStart, int rangeEnd) : base(node.Depth, node.Address)
        {
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
