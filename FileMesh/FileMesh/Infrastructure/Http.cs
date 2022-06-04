using FileMatch;
using Flurl.Http;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FileMesh.Infrastructure
{
    internal class Http
    {
        public const int AppPort = 8080;

        public static Task<T> Post<T>(Node node, string path, object body = null) {
            var url = $"http://{node.Address}:{AppPort}/{path}";
            return url.PostJsonAsync(body).ReceiveJson<T>();
        }

        public static Task<T> Get<T>(string address)
        {
            var url = $"http://{address}:{AppPort}";
            return url.WithTimeout(50).GetJsonAsync<T>();
        }

        public static Task<T> Get<T>(Node node, string path)
        {
            var url = $"http://{node.Address}:{AppPort}/{path}";
            return url.GetJsonAsync<T>();
        }

        public static string GetIp() {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostEntry(hostName).AddressList
                .FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork)
                .ToString();

            return myIP;
        }
    }
}
