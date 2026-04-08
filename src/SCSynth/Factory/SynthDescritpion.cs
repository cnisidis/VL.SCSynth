
using SCSynth.SCNodes;
using System.Reactive.Linq;
using VL.Core;
using VL.Core.Diagnostics;
using VL.Lib.Basics.Resources;


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

        public Guid id { get; set; }

        public SCManager.SynthDef synthDef { get; set; }
        public byte[] raw { get; set; }

        // Inputs and outputs
        List<PinDescription> inputs = new List<PinDescription>();
        List<PinDescription> outputs = new List<PinDescription>();

        
        public Dictionary<string, ControlParameter> parameters = new Dictionary<string, ControlParameter>();

        readonly IResourceProvider<SCManager> _managerProvider;

        public SynthDescritpion(IVLNodeDescriptionFactory factory, IResourceProvider<SCManager> managerProvider, SCManager.SynthDef synthdef)
        {

            this.synthDef = synthdef;   
            Factory = factory;
            FFullName = synthdef.name;
            Name = synthdef.name;
            FCategory = "SYNTHDEFS.";
            FSummary = synthdef.name;
            this.parameters = SynthCreator.BuildParameters(synthdef);
            

            _managerProvider = managerProvider;
            
            

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
                    parameters.Values.ForEach(parameter => {
                        
                        if(parameter.Type == ControlParameterType.Trigger)
                            GetTypeDefaultAndDescription((TriggerControlParameter)parameter, ref type, ref dflt, ref desc);
                            
                        else
                            GetTypeDefaultAndDescription((ValueControlParameter)parameter, ref type, ref dflt, ref desc);

                        inputs.Add(new PinDescription(parameter.Name, type, dflt, desc));
                    });

                   
                }
                else
                {
                    Console.WriteLine("This Synth has no Parameters exposed");
                }

                // Adds the Enable pin
                
                inputs.Add(new PinDescription("Enable", typeof(bool), true, "Enable the Synth"));
                //inputs.Add(new PinDescription("ResetAll", typeof(bool), false, "Reset All Parameters to their intial values"));

                
                //Adds the main Output Pin
                outputs.Add(new PinDescription(synthDef.name, typeof(Synth), null , "A Synth Node"));
                

                FInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void GetTypeDefaultAndDescription(ControlParameter parameter, ref Type type, ref object dflt, ref string desc)
        {

            string unit = "None";
            string[] featureNames = { };

            //desc = parameter.Name + "\n" + parameter.Value.ToString();
            
            if (parameter.Type == ControlParameterType.Trigger) {
                type = typeof(bool);
                dflt = parameter.InitValue==1f?true:false;
                desc = "Trigger Parameter";
            } 
            else
            {
                type = typeof(float);
                dflt = parameter.InitValue;
                desc = "Value Parameter";
            }
                

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
            //var managerHandler = _managerProvider.GetHandle();
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
