
using SCSynth.Factory;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VL.Lib.Collections;


namespace SCSynth.SCNodes
{
    public struct ParameterChangedEvent
    {
        
        public OldSynth synth;
        public Parameter param;
    }
    /// <summary>
    /// A SuperCollider Synth Instance.
    /// </summary>
    public class Synth:SCNode
    {
        /// <summary>
        /// SynthDef of the specific synth
        /// </summary>
        /// 
        public SCManager.SynthDef? SynthDef;
        /// <summary>
        /// Synth Name -> can be found in the synthdef.
        /// </summary>
        public string Name;

        private string _name;
        
        public Synth(SCManager.SynthDef? SynthDef, string Name="vvvv dummy synth"):base()
        {
            this.SynthDef = SynthDef;
            this.Name = this.SynthDef != null ? this.SynthDef.name : Name ;
            
            base.Initialize();
            
        }

        public override void Update()
        {
            if(_name != Name)
            {
                _name = Name;
                Invalidate();
            }
            base.Update();
        }

        
        
    }

    public class OldSynth : ISCNode
    {
        
        public string SynthDefName { get; set; }    
        public int scId { get; set; }
        public OldGroup ParentGroup { get; set; }
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

        public OldSynth(string SynthDefName)
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

        public OldSynth(string SynthDefName, Dictionary<string, Parameter> parameters)
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
