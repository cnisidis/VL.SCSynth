
using VL.Lib.Collections;
using static SCSynth.Factory.ASynth;

namespace SCSynth.Factory
{
    public class SynthCreator
    {

        public SCManager SCManager;
        public List<string> synthDefFolders { get; set; }

        public SynthCreator(SCManager? SCManager)
        {
            
            this.SCManager = SCManager == null ? new SCManager() : SCManager;
        }

        public static void CreateASynth(SCManager.SynthDef synthDef)
        {
            if (synthDef == null) return;

            var constant = new List<float>();
            var param_names = new List<SCManager.ParamName>();
            var param_init_vals = new List<float>();
            synthDef.GetParameters(out param_names, out param_init_vals);
            var ugenspecs = new List<SCManager.UgenSpec>();
            synthDef.GetUGensSpecs(out ugenspecs);


            //Create a new Synth Object to assert values
            ASynth synth = new ASynth();

            Dictionary<string, ASynth.Control> controls= new Dictionary<string, ASynth.Control>();

            bool hasTriggerControl = ugenspecs.FindAll(x => x.name == "TrigControl").Any();
            bool hasControl = ugenspecs.FindAll(x => x.name == "Control").Any();
            bool hasIputs = ugenspecs.FindAll(x => x.name == "In").Any();
            bool hasOutputs = ugenspecs.FindAll(x => x.name == "Out").Any();

            Dictionary<string, List<Tuple<SCManager.ParamName, float>>> parametersPerCategory = new() { {"trigger", new()}, {"simple", new() } };

            param_names.ForEach((x, idx) => { 
                if (x.Name.StartsWith("t_")){
                    parametersPerCategory["trigger"].Add(Tuple.Create(x, param_init_vals[idx]));
                }
                else
                {
                    parametersPerCategory["simple"].Add(Tuple.Create(x, param_init_vals[idx]));
                }
            });

            Console.WriteLine("\nMap SynthDef \n--- \nHas Controls: {0:B} \nHas Triggers: {1:B} \nHas In(s): {2:B} \nHas Out(s): {3:B} \n", hasControl, hasTriggerControl, hasIputs, hasOutputs);

            //Console.WriteLine("Trigger Parameters");
            //parametersPerCategory["trigger"].ForEach(x => { Console.WriteLine(x.Item1.Name); });
            //Console.WriteLine("Simple Parameters");
            //parametersPerCategory["simple"].ForEach(x => { Console.WriteLine(x.Item1.Name); });

            ugenspecs.ForEach(uspec => uspec.Inputs().Where(inp => !inp.isConstant).ForEach((inp, idx) => {
                if (ugenspecs[inp.Index].name=="Control")
                {
                    
                    Console.WriteLine("{0:G}\t\t--> {1:G}", parametersPerCategory["simple"][inp.ConstantIndex].Item1.Name, uspec.name);
                }
                else if (ugenspecs[inp.Index].name == "TrigControl")
                {
                    
                    Console.WriteLine("{0:G}\t\t--> {1:G}", parametersPerCategory["trigger"][inp.ConstantIndex].Item1.Name, uspec.name);
                }
            
            }));
            
            
            

            foreach (var control in controls.Values)
            {
                Console.WriteLine(control.ToString());
            }

        }

    }

    

    public class ASynth
    {
        

        public class Control
        {
            string Name;
            float Value;
            float InitValue;
            int Index;
            SCManager.UgenSpec? spec;
            public Control(string Name, int Index, float InitValue, SCManager.UgenSpec? UGenSpec=null)
            {
                this.Name = Name;
                this.Index = Index;
                this.Value = this.InitValue =  InitValue;
                this.spec = UGenSpec ==null ?  new SCManager.UgenSpec() : UGenSpec;
            }

            public string ToString()
            {
                return (Name +": "+ InitValue.ToString());
            }
            public static void CreateControlFromUGenSpec(SCManager.UgenSpec ugenspec, Tuple<List<SCManager.ParamName>, List<float>> parameters)
            {
                var ugenName = ugenspec.name;
                switch (ugenName) 
                {
                    case "TrigControl":
                        break;
                    case "Out": //Out.ar(bus, channels[])
                        var bus = ugenspec.Inputs().FirstOrDefault();
                        if (!bus.isConstant) {  }
                        break;
                }
            }
        }


        
        public class Input 
        { 
            
        }

        public class Output { }

        public class Trigger { }

        

        bool hasInputs;
        bool hasOutputs;
        bool Enable;

        

        SCManager.SynthDef synthDef;
        public List<Control> Controls;
        List<Input> inputs;
        List<Output> outputs;
        List<Trigger> triggers;

        public ASynth()
        {
            Controls=new List<Control>();
            inputs = new List<Input>();
            outputs = new List<Output>();
            triggers = new List<Trigger>();

        }

        public void Init() { }
        public void Update() { }

        public void Split(out Spread<Control> Controls)
        {
            Controls = this.Controls.ToSpread();
        }
    }
}
