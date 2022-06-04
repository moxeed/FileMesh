using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using FileMatch;
using FileMesh.Service;
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
            var term = HttpContext.GetRequestQueryData().GetKey(0);
            return MeshService.Search(term);
        }

        [Route(HttpVerbs.Get, "/Split")]
        public async Task<Index> Split()
        {
            var newChild = await HttpContext.GetRequestDataAsync<Node>();
            return MeshService.Split(newChild);
        }
    }
}