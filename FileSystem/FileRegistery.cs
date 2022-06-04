using FileMatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileSystem
{
    public class FileRegistery
    {
        private readonly IFileNetwork _fileNetwork;

        public ICollection<PhysicalFile> Files { get; }

        public FileRegistery(IFileNetwork fileNetwork)
        {
            Files = new HashSet<PhysicalFile>();
            _fileNetwork = fileNetwork;
        }

        public async Task Download(Entry entry) { 
            var file = await PhysicalFile.Download(entry);
            Files.Add(file);
        }
    }
}
