using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMatch
{
    public interface IGraphNetwork
    {
        Task Insert(Node node, Entry entry);
        Task<IEnumerable<Entry>> Search(Node node, string term);

        Task<IEnumerable<Node>> Scan();
        Task<Index> Split(Node parent, Node newChild);
    }
}
