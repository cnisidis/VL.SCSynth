
using SCSynth.SCNodes;
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
            

            this.synth = SynthCreator.CreateASynth(description.synthDef);
            
            Inputs = description.Inputs.Cast<PinDescription>().Select(pin => new Pin(pin.OriginalName, pin.Type) { Value = pin.DefaultValue}).ToArray();
            Outputs = description.Outputs.Select(pin => new Pin("Synth", typeof(Synth)) { Value = this.synth }).ToArray();
            
            
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
            foreach (var inputPin in Inputs.Cast<Pin>())
            {
                //Console.WriteLine("Name: {0} \n Originan: {1}", inputPin.Name, inputPin.OriginalName);
                if (inputPin.Type == typeof(float) || inputPin.Value.GetType() == typeof(Single) || inputPin.Value.GetType() == typeof(float))
                {
                    this.synth.ControlParameters[inputPin.OriginalName].Value = (float)inputPin.Value;
                }
                else if(inputPin.Type == typeof(bool) && inputPin.OriginalName.StartsWith("t_"))
                {

                    TriggerControlParameter tr = (TriggerControlParameter)this.synth.ControlParameters[inputPin.OriginalName];
                    tr.SetValue((bool)inputPin.Value);

                }
                else if (inputPin.Type == typeof(bool) && inputPin.OriginalName == "Enable")
                {
                    this.synth.Enabled = (bool)inputPin.Value;

                }
                
                
            }
            
        }

        public void Dispose()
        {
            Console.WriteLine("Disposed {0:G}", synth.SCId);
        }


    }
}
