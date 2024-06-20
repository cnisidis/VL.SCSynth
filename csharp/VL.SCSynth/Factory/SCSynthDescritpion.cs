
using System.Reactive.Linq;
using VL.Core;
using VL.Core.Diagnostics;
using VL.SCSynth;
using VL.SCSynth.Factory;

namespace VL.SCSynth.Factory
{
    internal class SCSynthDescritpion: IVLNodeDescription, IInfo
    {
        // Fields
        bool FInitialized;
        bool FError;
        
        string FFullName;
        string? FSummary;
        string FCategory;

        public SCSynth synth { get; }
        public string id { get; set; }

        public string synthDefName { get; set; }

        public string filepath { get; set; }

        // Inputs and outputs
        List<PinDescription> inputs = new List<PinDescription>();
        List<PinDescription> outputs = new List<PinDescription>();

        public SCSynthDescritpion(IVLNodeDescriptionFactory factory, string synthdefname, List<Parameter> parameters, string filepath)
        {

            Factory = factory;
            FFullName = synthdefname;
            Name = synthdefname;
            synthDefName = synthdefname;
            FCategory = "SYNTHDEFS.";
            FSummary = synthdefname;
            this.synth = new SCSynth(synthDefName);
            this.synth.Parameters = parameters.ToDictionary(x => x.Name);
            this.synth.synthDefFilePath = filepath;
            
            //FOperation = operation;

         }

        void Init()
        {
            this.synth.Id = Guid.NewGuid();
            if (FInitialized)
                return;

            try
            {
                Type type = typeof(object);
                object dflt = "";
                string name = "";
                string desc = "";

               if(this.synth.Parameters.Count > 0)
                {
                    foreach (var param in this.synth.Parameters.ToList())
                    {
                        GetTypeDefaultAndDescription(param.Value, ref type, ref dflt, ref desc);
                        inputs.Add(new PinDescription(param.Key, type, dflt, desc));
                        //For each LogicalChannel
                    }
                }
                else
                {
                    Console.WriteLine("This Synth has no Parameters exposed");
                }

                // Adds the trigger pin
                inputs.Add(new PinDescription("Reset All", typeof(bool), false, "Reset All Parameters to their default values"));
                inputs.Add(new PinDescription("Play", typeof(bool), false, "Play the Synth"));
                //inputs.Add(new PinDescription("ResetAll", typeof(bool), false, "Reset All Parameters to their intial values"));

                // For now let's just get the raw JSON response from Directus. Create a single string output pin
                
                outputs.Add(new PinDescription("Synth", typeof(SCSynth), this.synth, "Synth"));
                //outputs.Add(new PinDescription("Controls", typeof(Dictionary<String, FixtureNodeAttribute>), Controls, "Controls"));

                FInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void GetTypeDefaultAndDescription(Parameter parameter, ref Type type, ref object dflt, ref string desc)
        {

            string unit = "None";
            string[] featureNames = { };

            desc = parameter.Name + "\n" + parameter.Value.ToString();
            type = typeof(float);
            dflt = parameter.Value;

        }

        public IVLNodeDescriptionFactory Factory { get; }
        public string Name { get; }
        public string Category => FCategory;
        public bool Fragmented => false;
        public IReadOnlyList<IVLPinDescription> Inputs
        {
            get
            {
                Init();
                return inputs;
            }
        }
        public IReadOnlyList<IVLPinDescription> Outputs
        {
            get
            {
                Init();
                return outputs;
            }
        }

        public IEnumerable<VL.Core.Diagnostics.Message> Messages
        {
            get
            {
                if (FError)
                    yield return new Message(MessageType.Warning, "Grrrr");
                else
                    yield break;
            }
        }
        public string Summary => FSummary;
        public string Remarks => "";
        public IObservable<object> Invalidated => Observable.Empty<object>();
        public IVLNode CreateInstance(NodeContext context)
        {
            return new SynthNode(this, context);
        }
        public bool OpenEditor()
        {
            return true;
        }
        IReadOnlyList<IVLPinDescription> IVLNodeDescription.Inputs => Inputs;
        IReadOnlyList<IVLPinDescription> IVLNodeDescription.Outputs => Outputs;
    }
}
