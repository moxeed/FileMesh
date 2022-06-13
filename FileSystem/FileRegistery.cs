using FileMatch;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileSystem
{
    public class FileRegistery
    {
        private readonly IFileNetwork _fileNetwork;
        public ObservableCollection<PhysicalFile> Files { get; }

        public FileRegistery(IFileNetwork fileNetwork)
        {
            Files = new ObservableCollection<PhysicalFile>();
            _fileNetwork = fileNetwork;
        }

        public void Download(Entry entry, int chunkSize)
        {
            if (Files.Any(a => a.Id == entry.Id))
                return;
            var file = PhysicalFile.Download(entry, _fileNetwork, chunkSize);
            Files.Add(file);
        }

        public void AddFile(PhysicalFile file)
        {
            if (Files.Any(a => a.Id == file.Id))
                return;
            Files.Add(file);
        }

        public Task<Chunk> GetChunck(Guid id, int seq, int size)
        {
            foreach (var file in Files)
            {
                if (file.Id == id)
                {
                    return file.GetChunck(seq, size);
                }
            }

            throw new Exception("File Not Found");
        }
    }
}
