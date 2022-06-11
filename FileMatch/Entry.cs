using System;
using System.Collections.Generic;

namespace FileMatch
{
    public class Entry
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public EntryName Name { get; set; }
        public List<Node> Locations { get; set; }

        public Entry() {
            Locations = new List<Node>();
        }

        public Entry(Guid id, string name, long size)
        {
            Id = id;
            Size = size;
            Name = new EntryName(name);
            Locations = new List<Node>();
        }

        internal int Key => Name.Key;
    }
}
