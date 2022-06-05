using System.Collections.Generic;

namespace FileMatch.Model
{
    public class IndexModel
    {
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
        public IEnumerable<Entry> Entries { get; set; }
    }
}
