using FileMatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public async Task Download(Entry entry) { 
            var file = await PhysicalFile.Download(entry);
            Files.Add(file);
        }

        public void AddFile(PhysicalFile file)
        {
            Files.Add(file);
        }
    }
}
