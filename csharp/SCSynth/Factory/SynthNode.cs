
using VL.Core;


namespace SCSynth.Factory
{
    sealed class SynthNode: FactoryBasedVLNode, IVLNode
    {

        sealed class Pin : IVLPin
        {
            public object? Value { get; set; }
            public Type Type { get; }
            public string Name { get; }
            public string OriginalName { get; }
            public Pin(string name, Type type)
            {
                Type = type;
                Name = name;
                OriginalName = name;
            }

            
        }

        readonly SynthDescritpion description;



        
        public SynthNode(SynthDescritpion description, NodeContext nodeContext) : base(nodeContext)
        {
            
            
            this.description = description;
            
            //this.description = description;

            var SynthParameters = new Dictionary<string, Parameter>();

            foreach(var param in description.parameters)
            {
                var synthParam = new Parameter(param.Key, param.Value.initValue);
                SynthParameters.Add(param.Key, synthParam);
            }

            this.synth = new Synth(description.synthDefName, SynthParameters);
            this.synth.synthDefFilePath = description.filepath;
            Inputs = description.Inputs.Select(p => new Pin(p.Name, p.Type) { Value = p.DefaultValue}).ToArray();
            Outputs = description.Outputs.Select(p => new Pin("Synth", typeof(Synth)) { Value = this.synth }).ToArray();
            
            
        }

        public Synth synth;
        public IVLNodeDescription NodeDescription => description;

        public IVLPin[] Inputs { get; }

        public IVLPin[] Outputs { get; }
   

        public void Update()
        {

            if (!Inputs.Any())
                return;
            //Console.Write("Update");  
            foreach (var input in Inputs.Cast<Pin>())
            {
                //Console.WriteLine("Name: {0} \n Originan: {1}", input.Name, input.OriginalName);
                if (input.Type == typeof(float) || input.Value.GetType() == typeof(Single) || input.Value.GetType() == typeof(float))
                {
                    this.synth.Parameters[input.OriginalName].Value = (float)input.Value;
                }
                if(input.Type == typeof(bool) && input.OriginalName == "Enable")
                {
                    this.synth.isPlaying = (bool)input.Value;

                }
                
            }
            
        }

        public void Dispose()
        {
            Console.WriteLine("Disposed {0:G}", synth.scId);
        }


    }
}
