using FileMatch;
using FileMatch.Model;
using FileSystem;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace FileMesh.Controllers
{
    [ApiController]
    public sealed class MeshController : ControllerBase
    {
        [HttpGet("/")]
        public Node GetNode() => MeshService.GetNode();


        [HttpPost("/Insert")]
        public async Task Insert(Entry entry) {
            await MeshService.Insert(entry);
        }

        [HttpGet("/Search")]
        public Task<IEnumerable<Entry>> Search(string term)
        {
            return MeshService.Search(term);
        }

        [HttpPost("/Split")]
        public IndexModel Split(Node node)
        {
            return MeshService.Split(node);
        }

        [HttpGet("/File")]
        public Task<Chunk> File(Guid id, int seq, int size)
        {
            return MeshService.GetChunck(id, seq, size);
        }

        [HttpPost("/Download")]
        public Guid Download(Entry entry)
        {
            MeshService.DownloadFile(entry);
            return entry.Id;
        }
    }
}