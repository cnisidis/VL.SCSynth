﻿
using System.Reactive.Linq;
using VL.Core;
using VL.Core.Diagnostics;


namespace SCSynth.Factory
{
    internal class SynthDescritpion: IVLNodeDescription, IInfo
    {
        // Fields
        bool FInitialized;
        bool FError;
        
        string FFullName;
        string? FSummary;
        string FCategory;

        public Synth synth { get; set; }
        public Guid id { get; set; }

        public string synthDefName { get; set; }

        public string filepath { get; set; }

        // Inputs and outputs
        List<PinDescription> inputs = new List<PinDescription>();
        List<PinDescription> outputs = new List<PinDescription>();

        
        public Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();

        public SynthDescritpion(IVLNodeDescriptionFactory factory, string synthdefname, List<Parameter> parameters, string filepath)
        {

            Factory = factory;
            FFullName = synthdefname;
            Name = synthdefname;
            synthDefName = synthdefname;
            FCategory = "SYNTHDEFS.";
            FSummary = synthdefname;
            this.parameters = parameters.ToDictionary(x=>x.Name);
            this.filepath = filepath;
            
            

        }

        void Init()
        {
             
            if (FInitialized)
                return;

            try
            {
                Type type = typeof(object);
                object dflt = "";
                string name = "";
                string desc = "";

                
                if (parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        GetTypeDefaultAndDescription(param.Value, ref type, ref dflt, ref desc);
                        inputs.Add(new PinDescription(param.Key, type, dflt, desc));
                        Console.WriteLine(param.Key);
                        
                    }
                }
                else
                {
                    Console.WriteLine("This Synth has no Parameters exposed");
                }

                // Adds the trigger pin
                //inputs.Add(new PinDescription("Reset All", typeof(bool), false, "Reset All Parameters to their default values"));
                inputs.Add(new PinDescription("Enable", typeof(bool), true, "Enable the Synth"));
                //inputs.Add(new PinDescription("ResetAll", typeof(bool), false, "Reset All Parameters to their intial values"));

                // For now let's just get the raw JSON response from Directus. Create a single string output pin
                
                outputs.Add(new PinDescription("Synth", typeof(Synth), null , "Synth"));
                

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

            //desc = parameter.Name + "\n" + parameter.Value.ToString();
            desc = "Initial Value: " + parameter.Value.ToString();
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
                    yield return new Message(MessageType.Warning, "");
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
