using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using VL.Lib.IO.Notifications;

namespace VL.SCSynth
{

    public class Parameter
    {
        public string Name { get; set; }
        float initValue { get; set; }
        public float Value { get; set; }

        public int index { get; set; }

        public Parameter(string name = "Unnamed Parameter", float initValue = 0)
        {
            Name = name;
            this.initValue = initValue;
            this.Value = initValue;
        }

        public void Reset()
        {
            this.Value = initValue;
        }

        
    }
    public class Synth : ISCNode
    {
        /// <summary>
        /// Synth Def origins from
        /// </summary>
        public string SynthDefName { get; set; }    
        /// <summary>
        /// SuperCollider Node Id: this should always match the id on the SC node graph
        /// </summary>
        public int scId { get; set; }


        public string synthDefFilePath { get; set; }

        //Add synth parameters Enumerable* TODO 

        public Dictionary<string, Parameter> Parameters { get; set; }

        public AddActions AddAction { get; set; }
        public CallerInfo callerInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Synth(string SynthDefName)
        { 
            this.Parameters = new Dictionary<string, Parameter>();  
            this.SynthDefName = SynthDefName;
        }

        public void AutoID(CallerInfo caller)
        {
            throw new NotImplementedException();
        }

        public bool Notify(INotification notification, CallerInfo caller)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISCNode> GetInputs()
        {
            throw new NotImplementedException();
        }

        public void Identify(CallerInfo caller)
        {
            throw new NotImplementedException();
        }
    }
}
