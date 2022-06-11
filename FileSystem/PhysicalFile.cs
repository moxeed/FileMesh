using FileMatch;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
    public class PhysicalFile : INotifyPropertyChanged
    {
        const int WINDOW_SIZE = 5;
        private bool _isCompelete;

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; }
        public string Name { get; }
        public string Path { get; }
        public long Size { get; }
        public int ChunkSize { get; }
        public int LastWrittenChunkNumber { get; set; }
        public bool CanOpen => _isCompelete && !string.IsNullOrEmpty(Path);
        public float Progress
        {
            get
            {
                if (_isCompelete)
                    return 1;
                return (float)LastWrittenChunkNumber * ChunkSize / Size;
            }
        }

        int _downloadSpeed;
        public int DownloadSpeed
        {
            get
            {
                return _downloadSpeed;
            }
            set
            {
                _downloadSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DownloadSpeed)));
            }
        }

        internal async Task<Chunk> GetChunck(int seq, int size)
        {
            var data = new byte[size];
            var file = File.OpenRead(Path);
            file.Seek((seq - 1) * size, SeekOrigin.Begin);
            await file.ReadAsync(data, 0, size);

            return new Chunk(seq, data);
        }

        public PhysicalFile(string name, string path, long size)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
            Size = size;
            _isCompelete = true;
        }

        public PhysicalFile(Entry entry, int chunkSize = 65_536)
        {
            Id = entry.Id;
            Name = entry.Name.Name;
            Size = entry.Size;
            Path = $"{Directory.GetCurrentDirectory()}\\Data\\{Id}{System.IO.Path.GetExtension(Name)}";
            LastWrittenChunkNumber = 0;
            ChunkSize = chunkSize;
            _isCompelete = false;
        }

        public static PhysicalFile Download(Entry entry, IFileNetwork fileNetwork)
        {
            var file = new PhysicalFile(entry);

            var reqQueue = new Queue<int>();
            var resultQueue = new Queue<Chunk>();

            foreach (var node in entry.Locations)
            {
                file.DownloadChunk(fileNetwork, node, reqQueue, resultQueue).ConfigureAwait(false);
            }

            for (int i = 1; i <= WINDOW_SIZE; i++)
            {
                reqQueue.Enqueue(i);
            }

            file.WriteFile(reqQueue, resultQueue).ConfigureAwait(false);

            return file;
        }

        private async Task DownloadChunk(IFileNetwork fileNetwork, Node node, Queue<int> input, Queue<Chunk> output)
        {
            var seq = 0;
            while (seq != -1)
            {
                seq = await input.Dequeue();
                var chunck = await fileNetwork.GetChunk(node, Id, seq, ChunkSize);
                Console.WriteLine("Download " + seq);
                output.Enqueue(chunck);
            }
        }

        private async Task WriteFile(Queue<int> input, Queue<Chunk> output)
        {
            Directory.CreateDirectory("Data");
            using (File.Create(Path)) { }

            using (var stream = new FileStream(Path, FileMode.Append))
            {
                while (!IsCompelete)
                {
                    var chunk = await output.Dequeue();

                    if (chunk.SequenceNumber == LastWrittenChunkNumber + 1)
                    {
                        await stream.WriteAsync(chunk.Data, 0, chunk.Data.Length);
                        input.Enqueue(chunk.SequenceNumber + WINDOW_SIZE);
                        LastWrittenChunkNumber++;
                        Console.WriteLine("Writed " + chunk.SequenceNumber);
                    }
                    else
                    {
                        output.Enqueue(chunk);
                        await Task.Delay(100);
                    }
                }

                _isCompelete = true;
            }
        }

        private bool IsCompelete => LastWrittenChunkNumber * ChunkSize >= Size;
    }
}
