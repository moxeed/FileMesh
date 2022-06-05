using FileMatch;
using FileMatch.Model;
using FileMesh.Infrastructure;
using FileSystem;
using Rssdp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FileMesh.Service
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

        internal static async Task Initilize()
        {
            await Index.SelectParent();

            var deviceDefinition = new SsdpRootDevice()
            {
                CacheLifetime = TimeSpan.FromMinutes(30),
                Location = new Uri($"http://{Http.GetIp()}:{Index.Depth}"),
                DeviceTypeNamespace = nameof(FileMesh),
                DeviceType = nameof(Node),
                FriendlyName = nameof(FileMesh),
                Manufacturer = nameof(FileMesh),
                ModelName = nameof(FileMesh),
                DeviceVersion = 1,
                Uuid = Guid.NewGuid().ToString(), // persist in file
            };

            var publisher = new SsdpDevicePublisher();
            publisher.AddDevice(deviceDefinition);
        }

        internal static Task AddFile(FileResult file)
        {
            var size = file.OpenReadAsync().Result.Length;
            var entry = new Entry(Guid.NewGuid(), file.FileName, size);
            FileRegistery.AddFile(new PhysicalFile(file.FileName, file.FullPath, size));
            return Index.Insert(entry);
        }

        internal static Node GetNode()
        {
            var parentDepth = Index.Parent.Depth;
            return new Node(parentDepth + 1, Http.GetIp());
        }

        internal static Task Insert(Entry entry) => Index.Insert(entry);

        internal static Task<IEnumerable<Entry>> Search(string term)
        {
            var entryName = new EntryName(term);
            return Index.Search(entryName);
        }

        internal static IndexModel Split(Node newChild) => Index.Split(newChild);
    }
}
