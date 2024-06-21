﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using VL.Lib.IO.Notifications;

namespace VL.SCSynth
{

    
    public class SCSynth : ISCNode
    {
        
        public string SynthDefName { get; set; }    
        public int scId { get; set; }
        public string synthDefFilePath { get; set; }

        public bool isPlaying { get; set; }

        public Guid Id { get; set; }

        //Add synth parameters Enumerable* TODO 
        public Dictionary<string, Parameter> Parameters { get; set; }

        public AddActions AddAction { get; set; }

        

        public SCSynth(string SynthDefName)
        { 
            this.Parameters = new Dictionary<string, Parameter>();  
            this.SynthDefName = SynthDefName;
            this.Id = Guid.NewGuid();
            this.isPlaying = false;
            
        }

        public SCSynth(string SynthDefName, Dictionary<string, Parameter> parameters)
        {
            this.Parameters = parameters;
            this.SynthDefName = SynthDefName;
            this.Id = Guid.NewGuid();
            this.isPlaying = false;

        }


        public void ResetAll()
        {
            foreach(var param in  Parameters.Values) 
            { 
                param.Reset();
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public List<ISCNode> GetInputs()
        {
            return new List<ISCNode>();
        }

        
    }
}
