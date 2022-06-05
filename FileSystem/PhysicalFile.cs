using FileMatch;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
    public class PhysicalFile : INotifyPropertyChanged
    {
        private bool _isCompelete;

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; }
        public string Name { get; }
        public string Path { get; }
        public long Size { get; }
        public int ChunkSize { get; }
        public int LastWrittenChunkNumber { get; set; }
        public bool CanOpen => _isCompelete && !string.IsNullOrEmpty(Path);
        public float Progress { 
            get
            {
                if (_isCompelete)
                    return 1;
                return (float)LastWrittenChunkNumber * ChunkSize / Size;
            }
        }

        int _downloadSpeed;
        public int DownloadSpeed {
            get {
                return _downloadSpeed;
            }
            set {
                _downloadSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DownloadSpeed)));
            }
        }

        public PhysicalFile(string name, string path, long size)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
            Size = size;
            _isCompelete = true;
        }

        public PhysicalFile(Entry entry)
        {
            Id = entry.Id;
            Name = entry.Name.Name;  
            Size = entry.Size;
            Path = $"{Directory.GetCurrentDirectory()}/{Id}";
            _isCompelete = false;
        }

        public static Task<PhysicalFile> Download(Entry entry) {
            var file = new PhysicalFile(entry);

            return Task.FromResult(file);
        }
    }
}
