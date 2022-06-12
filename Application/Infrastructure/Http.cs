using FileMatch;
using Flurl.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    public class Http
    {
        public const int AppPort = 8080;

        public static async Task<T> Post<T>(Node node, string path, object body = null)
        {
            var url = $"http://{node.Address}:{AppPort}/{path}";
            try
            {
                var res = await url.PostJsonAsync(body).ReceiveJson<T>();
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<T> Get<T>(Node node, string path)
        {
            var url = $"http://{node.Address}:{AppPort}/{path}";
            var res = await url.GetJsonAsync<T>();
            return res;
        }

        public static string GetIp()
        {
            string hostName = Dns.GetHostName();
            var myIP = Dns.GetHostEntry(hostName).AddressList
                .FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);

            if (myIP is null)
            {
                return "localhost";
            }

            return myIP.ToString();
        }
    }
}
