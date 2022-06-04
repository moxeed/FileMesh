using FileMatch;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
    public class PhysicalFile
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Path { get; }
        public int Size { get; }
        public int ChunkSize { get; }
        public int LastWrittenChunkNumber { get; set; }

        public PhysicalFile(string name, string path, int size)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
            Size = size;
        }

        public PhysicalFile(Entry entry)
        {
            Id = entry.Id;
            Name = entry.Name.Name;  
            Size = entry.Size;
            Path = $"{Directory.GetCurrentDirectory()}/{Id}";
        }

        public static Task<PhysicalFile> Download(Entry entry) {
            var file = new PhysicalFile(entry);

            return Task.FromResult(file);
        }
    }
}
