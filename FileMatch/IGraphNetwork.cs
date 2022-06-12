using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMatch
{
    public interface IGraphNetwork
    {
        Task Insert(Node node, Node source, Entry entry);
        Task<IEnumerable<Entry>> Search(Node node, string term);
        Task<IEnumerable<Node>> Scan();
        Task Reset(Node node, int start, int end);
        Task Join(Node parent, Node node);
    }
}
