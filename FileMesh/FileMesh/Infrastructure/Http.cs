﻿using FileMatch;
using Flurl.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FileMesh.Infrastructure
{
    internal class Http
    {
        public const int AppPort = 8080;

        public static async Task<T> Post<T>(Node node, string path, object body = null) {
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

        public static string GetIp() {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostEntry(hostName).AddressList
                .FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork && i.ToString().StartsWith("192"))
                .ToString();

            return myIP;
        }
    }
}
