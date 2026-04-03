using SCSynth.GraphNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSynth.Factory;
using System.Diagnostics.Contracts;
using VL.Lib.Collections;

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

            Console.WriteLine("\nMap SynthDef \n--- \nHas Controls: {0:B} \nHas Triggers: {1:B} \nHas In(s): {2:B} \nHas Out(s): {3:B} \n", hasControl, hasTriggerControl, hasIputs, hasOutputs);

            param_names.ForEach((p, idx) =>
            {
                var pname = p.Name;
                if(p.Name.StartsWith("t_")) pname = p.Name.Substring(2)+" Trigger";
                controls.TryAdd(p.Name , new ASynth.Control(pname, p.Index, param_init_vals[p.Index]));
            });

            
            
            if(hasIputs || hasOutputs)
            {
                foreach (var ugenspec in ugenspecs)
                {
                    //Console.WriteLine("InpIndex:{0:G}   InputName:{1:G}   ParamName:{2:G}   Value: {3:G}",
                    //                inp.Index,
                    //                ugenspecs[inp.Index].name,
                    //                //ugenspecs[inp.Index].Otuputs()[inp.ConstantIndex].calcRate, 
                    //                param_names[inp.ConstantIndex].Name,
                    //                param_init_vals[param_names[inp.ConstantIndex].Index]
                    //                );
                    if (ugenspec.name == "In")
                    {
                        Console.WriteLine("In|Inputs->: " + ugenspec.Inputs().Count);
                        foreach (var inp in ugenspec.Inputs())
                        {
                            if (!inp.isConstant && ugenspecs[inp.Index].name == "Control")
                            {
                                var paramToUpdate = param_names[inp.ConstantIndex].Name;
                                
                                controls.TryGetValue(paramToUpdate, out var ctrl);
                                if (ctrl != null) { Console.WriteLine("Create Bus"); }
                            }
                                
                        }
                        

                        Console.WriteLine("In|Outputs->: " + ugenspec.Inputs().Count);
                    }
                    if (ugenspec.name == "Out")
                    {
                        Console.WriteLine("Out|Inputs->: "+ugenspec.Inputs().Count);
                        foreach(var inp in ugenspec.Inputs())
                        {
                            if(!inp.isConstant && ugenspecs[inp.Index].name == "Control")
                            {
                                Console.WriteLine(param_names[inp.ConstantIndex].Name);
                            }
                               
                        }
                        
                    }
                }
            }

            foreach (var control in controls.Values)
            {
                Console.WriteLine(control.ToString());
            }

        }

    }

    public class SCUGen
    {
        SCManager.UgenSpec specs;


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
