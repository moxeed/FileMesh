using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System.Threading.Tasks;

namespace Draft.Controllers
{
    public sealed class MeshController : WebApiController
    {
        [Route(HttpVerbs.Get, "/people")]
        public Task<string> GetAllPeople() => Task.FromResult("ok");
    }
}