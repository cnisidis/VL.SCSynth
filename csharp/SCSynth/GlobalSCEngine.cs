using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSynth
{
    public class GlobalSCEngine
    {
        /// <summary>
        /// Synthdef (synthdefname, synthdefpath)
        /// </summary>
        public Dictionary<String, String> SynthDefs { get; set; }
        /// <summary>
        /// server scsynth filepath (ex. C:/ProgramFiles/Supercollider/scsynth.exe)
        /// </summary>
        public string SCSynthFilePath { get; set; }


        /// <summary>
        /// Nodegaph to be defined / one per session
        /// </summary>
        public object NodeGraph { get; set; }
        /// <summary>
        /// Total Synths in current NodeGraph
        /// </summary>
        public int SynthsCount { get; set; }
        /// <summary>
        /// Global Engine Constructor
        /// </summary>
        public GlobalSCEngine()
        {
            SynthDefs = new Dictionary<String, String>();   
        }

        

    }
}
