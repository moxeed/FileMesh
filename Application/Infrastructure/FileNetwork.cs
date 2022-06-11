using FileMatch;
using FileSystem;
using System;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    internal class FileNetwork : IFileNetwork
    {
        public Task<Chunk> GetChunk(Node node, Guid id, int sequenceNumber, int size) => Http.Get<Chunk>(node, $"File?id={id}&seq={sequenceNumber}&size={size}");
    }
}
