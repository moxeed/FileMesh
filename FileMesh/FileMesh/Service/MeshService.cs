using FileMatch;
using FileMesh.Infrastructure;
using FileSystem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMesh.Service
{
    static internal class MeshService
    {
        public static Index _index;
        public static FileRegistery _fileRegistery;

        static MeshService() {

            _index = new Index(new GraphNetwork(), new Node(0, Http.GetIp()));
            _fileRegistery = new FileRegistery(new FileNetwork());

            _index.SelectParent();
        }

        internal static Node GetNode()
        {
            var parentDepth = _index.Parent.Depth;
            return new Node(parentDepth + 1, Http.GetIp());
        }

        internal static Task Insert(Entry entry) => _index.Insert(entry);

        internal static Task<IEnumerable<Entry>> Search(string term) {
            var entryName = new EntryName(term);
            return _index.Search(entryName);
        }

        internal static Index Split(Node newChild) => _index.Split(newChild);
    }
}
