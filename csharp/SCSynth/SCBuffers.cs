


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

    public class SCBuffer
    {
        public int scID { get; set; }

        private String FilePath { get; set; }

        public int FileStartFrame { get; set; } 
        public int BufferStartFrame { get; set; }

        public int TotalFramesToRead { get; set; }

        public bool LeaveFileOpen { get; set; }

        

        public SCBuffer(string FilePath)
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

    }
}
