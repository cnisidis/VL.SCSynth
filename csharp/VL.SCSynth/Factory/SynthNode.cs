
using VL.Core;
using VL.Core.Diagnostics;

namespace VL.SCSynth.Factory
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

        readonly SCSynthDescritpion description;



        // This is where we'll run the queries to the Directus instance
        public SynthNode(SCSynthDescritpion description, NodeContext nodeContext) : base(nodeContext)
        {
            this.description = description;
            Inputs = description.Inputs.Select(p => new Pin(p.Name, p.Type) { Value = p.DefaultValue }).ToArray();
            Outputs = description.Outputs.Select(p => new Pin(p.Name, p.Type) { Value = p.DefaultValue }).ToArray();
        }
        public IVLNodeDescription NodeDescription => description;

        public IVLPin[] Inputs { get; }

        public IVLPin[] Outputs { get; }
   

        public void Update()
        {
            //Console.Write("Update");  
        }

        public void Dispose()
        {
            Console.WriteLine("Byyyye");
        }


    }
}
