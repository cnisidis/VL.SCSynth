


using SCSynth.Messaging;
using System.Reactive.Subjects;


namespace SCSynth
{
    public enum SCBufferFileType
    {
        aiff,
        next,
        wav,
        ircam,
        raw
    }

    public enum SCBufferFormat
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

    public class SCBuffer : IObservable<SCMessage>
    {
        public int bufferNumber;
        public int numberOfFrames;
        public int numberOfChannels;
        public float sampleRate;
        public string filePath;

        public bool hasFilePath { get => 
            
                ((VL.Lib.IO.Path)filePath).Exists && ((VL.Lib.IO.Path)filePath).IsFile && ((VL.Lib.IO.Path)filePath).NameWithExtension.Split(".")[1].Contains("wav");
             
        }

        public int startingFrame;
        public int numberOfFramesToRead;


        public bool? leaveFileOpen;

        public SCBufferFileType headerFormat;
        public SCBufferFormat sampleFormat;

        private readonly Subject<SCMessage> _message = new();
        public IObservable<SCMessage> message => _message;

        public bool AutoLoad;

        public SCBuffer(string? filePath, bool autoLoad=false)
        {
            this.filePath = filePath;
            AutoLoad = autoLoad;
            if (autoLoad) 
            {
                Read();
            }
        }

        public static SCBuffer BufferFromFile(string filePath)
        {
            return new SCBuffer(filePath);
        }


        /// <summary>
        /// Read sound file data into an existing buffer. (Asynchronous)
        /// </summary>
        public void Read()
        {
            if(this.filePath!=null)
            {
                _message.OnNext(new SCMessage(SCMessageType.BufferScalar,""));
            }
            else
            {
                Console.WriteLine("First assign a file path before reading a buffer");
            }
        }
        /// <summary>
        /// Write sound file data.
        /// </summary>
        /// <param name="filePath"></param>
        public void Write(string filePath)
        {

        }
        /// <summary>
        /// Allocate buffer space.
        /// </summary>
        public void Alloc()
        {


        }
        /// <summary>
        /// Allocate buffer space and read a sound file.
        /// </summary>
        public void AllocRead()
        {

        }
        /// <summary>
        /// Allocate buffer space and read channels from a sound file.
        /// </summary>
        public void AllocReadChannel()
        {

        }
        /// <summary>
        /// Free buffer data.
        /// </summary>
        public void Free()
        {

        }
        /// <summary>
        /// Zero sample data.
        /// </summary>
        public void Zero()
        {

        }
        /// <summary>
        /// Set sample value(s).
        /// </summary>
        public void Set()
        {

        }
        /// <summary>
        /// Set ranges of sample value(s).
        /// </summary>
        public void SetRange()
        {

        }
        /// <summary>
        /// Fill ranges of sample value(s).
        /// </summary>
        public void Fill()
        {

        }
        
        public void Gen()
        {

        }

        /// <summary>
        /// Get buffer info.
        /// </summary>
        public void Query()
        {

        }

        /// <summary>
        /// Get sample value(s).
        /// </summary>
        public void Get()
        {

        }
        /// <summary>
        /// Get ranges of sample value(s).
        /// </summary>
        public void GetRange()
        {

        }
        public IDisposable Subscribe(IObserver<SCMessage> observer) => _message.Subscribe(observer);
    }
}
