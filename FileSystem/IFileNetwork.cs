using FileMatch;
using System;
using System.Threading.Tasks;

namespace FileSystem
{
    public interface IFileNetwork
    {
        Task<Chunk> GetChunk(Node node, Guid id, int sequenceNumber, int size);
    }
}
