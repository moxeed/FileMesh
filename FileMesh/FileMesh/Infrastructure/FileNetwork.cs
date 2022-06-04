using FileMatch;
using FileSystem;
using System;
using System.Threading.Tasks;

namespace FileMesh.Infrastructure
{
    internal class FileNetwork : IFileNetwork
    {
        public Task<Chunk> GetChunk(Node node, Guid id, int sequenceNumber) => Http.Get<Chunk>(node, $"File/{id}/{sequenceNumber}");
    }
}
