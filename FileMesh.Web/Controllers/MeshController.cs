using FileMatch;
using FileSystem;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

namespace FileMesh.Controllers
{
    [ApiController]
    [EnableCors(nameof(CorsPolicy))]
    public sealed class MeshController : ControllerBase
    {
        [HttpGet("/")]
        public Node GetNode() => MeshService.GetNode();


        [HttpPost("/Insert")]
        public async Task Insert(InsertModel model) {
            await MeshService.Insert(model.Entry, model.Source);
        }

        [HttpGet("/Search")]
        public Task<IEnumerable<Entry>> Search(string term)
        {
            return MeshService.Search(term);
        }

        [HttpPost("/Join")]
        public Task<bool> Join(Node node)
        {
            return MeshService.Join(node);
        }

        [HttpGet("/Reset")]
        public string Reset()
        {
            return MeshService.Reset();
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

        [HttpPost("/Health")]
        public bool Health(Node node)
        {
            return MeshService.Health(node);
        }
    }
}