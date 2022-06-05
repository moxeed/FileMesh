using System.ComponentModel;

namespace FileMatch
{
    public class Node : INotifyPropertyChanged
    {
        int _depth;
        public int Depth
        {
            get
            {
                return _depth;
            }
            set
            {
                _depth = value;
                PropertyChange(nameof(Depth));
            }
        }
        public string Address { get; set; }

        public Node(int depth, string address)
        {
            Depth = depth;
            Address = address;
        }

        public event PropertyChangedEventHandler PropertyChanged;
            
        protected void PropertyChange(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
