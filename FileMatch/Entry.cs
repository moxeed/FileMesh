using System;
using System.Collections.Generic;

namespace FileMatch
{
    public class Entry
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public List<Node> Locations { get; set; }

        public Entry() {
            Locations = new List<Node>();
        }

        public Entry(Guid id, string name, long size)
        {
            Id = id;
            Size = size;
            Name = name;
            Locations = new List<Node>();
        }

        public bool IsMatch(string term) => Name.ToLower().Contains(term);
    }
}
