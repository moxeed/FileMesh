using System.Linq;

namespace FileMatch
{
    public class EntryName
    {
        public string Name { get; }
        internal int Key => Name.Min();

        public EntryName(string name)
        {
            Name = name;
        }

        public bool IsMatch(EntryName other) => Name.Contains(other.Name);
    }
}
