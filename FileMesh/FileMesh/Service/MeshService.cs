using FileMatch;
using FileMesh.Infrastructure;
using FileSystem;
using Rssdp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileMesh.Service
{
    static internal class MeshService
    {
        public static Index _index;
        public static FileRegistery _fileRegistery;

        static MeshService()
        {

            _index = new Index(new GraphNetwork(), new Node(0, Http.GetIp()));
            _fileRegistery = new FileRegistery(new FileNetwork());
        }

        internal static async Task Initilize()
        {
            await _index.SelectParent();

            var deviceDefinition = new SsdpRootDevice()
            {
                CacheLifetime = TimeSpan.FromMinutes(30),
                Location = new Uri($"http://{Http.GetIp()}:{_index.Depth}"),
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

        internal static Node GetNode()
        {
            var parentDepth = _index.Parent.Depth;
            return new Node(parentDepth + 1, Http.GetIp());
        }

        internal static Task Insert(Entry entry) => _index.Insert(entry);

        internal static Task<IEnumerable<Entry>> Search(string term)
        {
            var entryName = new EntryName(term);
            return _index.Search(entryName);
        }

        internal static Index Split(Node newChild) => _index.Split(newChild);
    }
}
