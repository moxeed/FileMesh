using FileMatch;
using FileMatch.Model;
using FileSystem;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.ObjectModel;
using Service.Models;

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

        [HttpPost("/Download")]
        public Guid Download(Entry entry)
        {
            MeshService.DownloadFile(entry);
            return entry.Id;
        }

        [HttpGet("/File")]
        public Task<Chunk> File(Guid id, int seq, int size)
        {
            return MeshService.GetChunck(id, seq, size);
        }

        [HttpGet("/List")]
        public ObservableCollection<PhysicalFile> List()
        {
            return MeshService.GetFiles();
        }


        [HttpPost("/Add")]
        public Task Add(FileModel file)
        {
            return MeshService.AddFile(file);
        }
    }
}