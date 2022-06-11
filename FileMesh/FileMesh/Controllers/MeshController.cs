using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using FileMatch;
using FileMatch.Model;
using FileSystem;
using Service;
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
        public async Task Insert() {
            var entry = await HttpContext.GetRequestDataAsync<Entry>();
            await MeshService.Insert(entry);
        }

        [Route(HttpVerbs.Get, "/Search")]
        public Task<IEnumerable<Entry>> Search()
        {
            var term = HttpContext.GetRequestQueryData().GetValues("term")[0];
            return MeshService.Search(term);
        }

        [Route(HttpVerbs.Post, "/Split")]
        public async Task<IndexModel> Split()
        {
            var newChild = await HttpContext.GetRequestDataAsync<Node>();
            return MeshService.Split(newChild);
        }

        [Route(HttpVerbs.Get, "/File")]
        public Task<Chunk> File([QueryField] Guid id, [QueryField] int seq, [QueryField] int size)
        {
            return MeshService.GetChunck(id, seq, size);
        }
    }
}