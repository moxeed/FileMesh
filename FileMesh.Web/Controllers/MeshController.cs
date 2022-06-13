using FileMatch;
using FileSystem;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;
using System.Collections.ObjectModel;

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

        [HttpPost("/Download")]
        public Guid Download(DownloadModel model)
        {
            MeshService.DownloadFile(model.Entry, model.ChunckSize);
            return model.Entry.Id;
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

        [HttpPost("/Health")]
        public bool Health(Node node)
        {
            return MeshService.Health(node);
        }

        [HttpPost("/Upload")]
        public async Task<bool> Upload([FromForm] IFormFile file) 
        {
            var path = $"{Directory.GetCurrentDirectory()}\\Data\\{file.FileName}";
            Directory.CreateDirectory("Data");
            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            await MeshService.AddFile(new FileModel
            {
                Size = file.Length,
                FileName = file.FileName,
                FullPath = path,
            });

            return true;
        }
    }
}