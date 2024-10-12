
namespace SCSynth
{

    
    public class Synth : ISCNode
    {
        
        public string SynthDefName { get; set; }    
        public int scId { get; set; }

        public Guid Id { get; set; }
        public string synthDefFilePath { get; set; }

        public bool isPlaying { get; set; }

        

        //Add synth parameters Enumerable* TODO 
        public Dictionary<string, Parameter> Parameters { get; set; }

        public AddActions AddAction { get; set; }

        

        public Synth(string SynthDefName)
        { 
            this.Parameters = new Dictionary<string, Parameter>();  
            this.SynthDefName = SynthDefName;
            this.Id = Guid.NewGuid();
            this.isPlaying = false;
            
        }

        public Synth(string SynthDefName, Dictionary<string, Parameter> parameters)
        {
            this.Parameters = parameters;
            this.SynthDefName = SynthDefName;
            this.Id = Guid.NewGuid();
            this.isPlaying = false;

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

        public IEnumerable<ISCNode> GetInputs()
        {
            return null;
        }
        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }

        public void SetSCId(int index)
        {
            this.scId = index;
        }

        public int GetSCId()
        {
            return this.scId;
        }

    }
}
