using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMatch
{
    public interface IGraphNetwork
    {
        Task Insert(Node node, Node source, Entry entry);
        Task<IEnumerable<Entry>> Search(Node node, string term);
        Task<IEnumerable<Node>> Scan();
        Task<string> Reset(Node node);
        Task Join(Node parent, Node node);
        Task Health(Node parent, Node index);
    }
}
