﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FileMatch
{
    public class Entry
    {
        public Guid Id { get; }
        public int Size { get; }
        public EntryName Name { get; }
        public ICollection<Node> Locations { get; }

        public Entry(Guid id, string name, int size)
        {
            Id = id;
            Size = size;
            Name = new EntryName(name);
            Locations = new HashSet<Node>();
        }

        internal int Key => Name.Key;
    }
}