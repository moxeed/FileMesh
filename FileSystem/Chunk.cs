namespace FileSystem
{
    public class Chunk
    {
        public int SequenceNumber { get; }
        public byte[] Data { get; }

        public Chunk(int sequenceNumber, byte[] data)
        {
            SequenceNumber = sequenceNumber;
            Data = data;
        }
    }
}
