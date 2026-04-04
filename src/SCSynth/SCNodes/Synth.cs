
using VL.Lib.Collections;
using System.Reactive.Subjects;
using System.Reactive.Linq;


namespace SCSynth.SCNodes
{
    public struct ParameterChangedEvent
    {
        
        public Synth synth;
        public Parameter param;
    }

    public class Synth : ISCNode
    {
        
        public string SynthDefName { get; set; }    
        public int scId { get; set; }
        public Group ParentGroup { get; set; }
        public Guid Id { get; set; }
        public string synthDefFilePath { get; set; }
        public bool hasChildren => false;
        public bool Enabled { get; set; }

        public bool isPlaying { get; set; }

        private byte[] _rawData;

        //Add synth parameters Enumerable* TODO 
        public Dictionary<string, Parameter> Parameters { get; set; }

        public AddActions AddAction { get; set; }
        public int Order { get; set; }

        public Synth(string SynthDefName)
        { 
            Parameters = new Dictionary<string, Parameter>();  
            this.SynthDefName = SynthDefName;
            Id = Guid.NewGuid();
            isPlaying = false;
            ParentGroup = null;

            // Wire every parameter to the central stream
            foreach (var param in Parameters.Values)
            {
                param.OnChanged = (param) =>
                {
                    _parameterStream.OnNext(new ParameterChangedEvent
                    {
                       synth = this,
                       param = param,
                    });
                };
            }

        }

        public Synth(string SynthDefName, Dictionary<string, Parameter> parameters)
        {
            Parameters = parameters;
            this.SynthDefName = SynthDefName;
            Id = Guid.NewGuid();
            isPlaying = false;
            ParentGroup = null;

            // Wire every parameter to the central stream
            foreach (var param in Parameters.Values)
            {
                param.OnChanged = (param) =>
                {
                    _parameterStream.OnNext(new ParameterChangedEvent
                    {
                        
                        synth = this,
                        param = param,
                    });
                };
            }

        }


        public void ResetAll()
        {
            foreach(var param in  Parameters.Values) 
            { 
                param.Reset();
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public List<ISCNode> GetInputs()
        {
            return new List<ISCNode>();
        }
        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }


        public Spread<byte> GetBytes()
        {
            return _rawData.ToSpread();
        }

        // The single stream vvvv gamma will watch
        private readonly Subject<ParameterChangedEvent> _parameterStream = new Subject<ParameterChangedEvent>();
        public IObservable<ParameterChangedEvent> ParameterStream => _parameterStream;

        
        
        

    }
}
