using FileMatch;

namespace Service.Models
{
    public class DownloadModel
    {
        public Entry Entry { get; set; }

        public int ChunckSize { get; set; }
    }
}
