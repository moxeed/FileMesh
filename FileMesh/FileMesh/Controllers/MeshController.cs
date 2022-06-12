using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using FileMatch;
using FileSystem;
using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMesh.Controllers
{
    public sealed class MeshController : WebApiController
    {
        [Route(HttpVerbs.Get, "/")]
        public Node GetNode() => MeshService.GetNode();


        [Route(HttpVerbs.Post, "/Insert")]
        public async Task<bool> Insert() {
            var model = await HttpContext.GetRequestDataAsync<InsertModel>();
            return await MeshService.Insert(model.Entry, model.Source);
        }

        [Route(HttpVerbs.Get, "/Search")]
        public Task<IEnumerable<Entry>> Search()
        {
            var term = HttpContext.GetRequestQueryData().GetValues("term")[0];
            return MeshService.Search(term);
        }

        [Route(HttpVerbs.Post, "/Join")]
        public async Task Join()
        {
            var node = await HttpContext.GetRequestDataAsync<Node>();
            await MeshService.Join(node);
        }

        [Route(HttpVerbs.Get, "/Reset")]
        public string Reset()
        {
            return MeshService.Reset();
        }

        [Route(HttpVerbs.Get, "/File")]
        public Task<Chunk> File([QueryField] Guid id, [QueryField] int seq, [QueryField] int size)
        {
            return MeshService.GetChunck(id, seq, size);
        }

        [Route(HttpVerbs.Post, "/Health")]
        public async Task<bool> Health()
        {
            var node = await HttpContext.GetRequestDataAsync<Node>();
            return MeshService.Health(node);
        }
    }
}