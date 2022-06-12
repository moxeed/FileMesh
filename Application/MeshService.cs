using FileMatch;
using FileSystem;
using Rssdp;
using Service.Infrastructure;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public static class MeshService
    {
        public static Index Index;
        public static FileRegistery FileRegistery;

        static MeshService()
        {
            Index = new Index(new GraphNetwork(), new Node(0, Http.GetIp()));
            FileRegistery = new FileRegistery(new FileNetwork());
        }

        public static async Task Initilize()
        {
            await Index.SelectParent();
            _ = Task.Run(Index.Heart);

            var deviceDefinition = new SsdpRootDevice()
            {
                CacheLifetime = TimeSpan.FromMinutes(30),
                Location = new Uri($"http://{Http.GetIp()}:{Index.Depth}"),
                DeviceTypeNamespace = nameof(Service),
                DeviceType = nameof(Node),
                FriendlyName = nameof(Service),
                Manufacturer = nameof(Service),
                ModelName = nameof(Service),
                DeviceVersion = 1,
                Uuid = Guid.NewGuid().ToString(), // persist in file
            };

            var publisher = new SsdpDevicePublisher();
            publisher.AddDevice(deviceDefinition);
        }

        public static Task<Chunk> GetChunck(Guid id, int seq, int size)
        {
            return FileRegistery.GetChunck(id, seq, size);
        }

        public static Task AddFile(FileModel file)
        {
            var physicalFile = new PhysicalFile(file.FileName, file.FullPath, file.Size);
            var entry = new Entry(physicalFile.Id, file.FileName, file.Size);

            FileRegistery.AddFile(physicalFile);
            return Index.Insert(entry);
        }

        public static void DownloadFile(Entry entry)
        {
            FileRegistery.Download(entry);
        }

        public static Node GetNode()
        {
            var parentDepth = Index.Parent.Depth;
            return new Node(parentDepth + 1, Http.GetIp());
        }

        public static Task<bool> Insert(Entry entry, Node source) => Index.Insert(entry, source);

        public static Task<IEnumerable<Entry>> Search(string term)
        {
            return Index.Search(term);
        }

        public static string Reset() => Index.Reset();
        public static Task<bool> Join(Node child) => Index.Join(child);
        public static bool Health(Node child) => Index.Health(child);
    }
}
