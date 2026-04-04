


using SCSynth.SCNodes;
using VL.Lib.IO;

namespace SCSynth
{
    public enum BufferFileTypes
    {
        aiff,
        next,
        wav,
        ircam,
        raw
    }

    public enum BufferFormats
    {
        int8,
        int16,
        int24,
        int32,
        @float,
        @double,
        mulaw,
        alaw

    }

    public class Buffer:ISCNode
    {
        public int scID { get; set; }
        public int Order { get; set; }
        public Group ParentGroup { get; set; }
        private String FilePath { get; set; }

        public int FileStartFrame { get; set; } 
        public int BufferStartFrame { get; set; }
        public bool hasChildren => false;
        public int TotalFramesToRead { get; set; }

        public bool LeaveFileOpen { get; set; }
        public int scId { get; set; }
        public Guid Id { get; set; }
        public AddActions AddAction { get; set; }
        public bool Enabled { get; set; }
        public Buffer(string FilePath)
        {
            FileStartFrame = 0;
            BufferStartFrame = 0;
            TotalFramesToRead = -1;
            LeaveFileOpen = false;
            this.FilePath = FilePath;
        }


        public void SetFilePath(string filePath)
        {
            FilePath = filePath;
        }

        public string GetFilePath()
        {
            return FilePath;
        }

        public IEnumerable<byte> GetBytes()
        {
            byte[] file = System.IO.File.ReadAllBytes(FilePath);
            return file;
        }

        public List<ISCNode> GetInputs()
        {
            throw new NotImplementedException();
        }
    }
}
