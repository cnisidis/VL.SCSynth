using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;

namespace VL.SCSynth.Factory
{
    internal class SynthNode: FactoryBasedVLNode, IVLNode
    {
        readonly SCSynthDescritpion description;
        readonly Pin? resultPin;
        readonly Pin? resetPin;
        readonly Pin? runPin;


        // This is where we'll run the queries to the Directus instance
        public SynthNode(SCSynthDescritpion description, NodeContext nodeContext) : base(nodeContext)
        {



            this.description = description;
            Inputs = description.Inputs.Select(p => new Pin() { Name = p.Name, OriginalName = ((PinDescription)p).OriginalName, Type = p.Type, Value = p.DefaultValue }).ToArray();
            Outputs = description.Outputs.Select(p => new Pin() { Name = p.Name, OriginalName = ((PinDescription)p).OriginalName, Type = p.Type, Value = p.DefaultValue }).ToArray();

            //resultPin = Outputs.FirstOrDefault(o => o.Name == "Result");
            resultPin = Outputs.FirstOrDefault(o => o.Name == "Synth");
            runPin = Inputs.LastOrDefault();
            //resetPin = Inputs.LastOrDefault();
            //controlsPin = Outputs.FirstOrDefault(o => o.Name == "Controls");

            // Create RestClient & RestRequest


            // Look for authentication stuff
            try
            {

                Console.WriteLine("Added auth parameter ");
            }
            catch (Exception ex)
            {
                //authParameterName = "";
                Console.WriteLine("No query-based authentication found");
            }
        }
        public IVLNodeDescription NodeDescription => description;

        public Pin[] Inputs { get; }
        public Pin[] Outputs { get; }

        public void Update()
        {
            //Console.Write("Update");  
        }

        public void Dispose()
        {
            Console.WriteLine("Byyyye");
        }

        IVLPin[] IVLNode.Inputs => Inputs;
        IVLPin[] IVLNode.Outputs => Outputs;

    }
}
