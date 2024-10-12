using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Basics.Resources;

namespace SCSynth
{
    public class GlobalSCEngine : IDisposable
    {
        /// <summary>
        /// Synthdef (synthdefname, synthdefpath)
        /// </summary>
        public static Dictionary<String, SynthDefEntry> SynthDefEntries = new Dictionary<String, SynthDefEntry>();  
        /// <summary>
        /// server scsynth filepath (ex. C:/ProgramFiles/Supercollider/scsynth.exe)
        /// </summary>
        public static string SCSynthFilePath { get; set; }


        /// <summary>
        /// Nodegaph to be defined / one per session
        /// </summary>
        public static object NodeGraph { get; set; }
        /// <summary>
        /// Total Synths in current NodeGraph
        /// </summary>
        public static int SynthsCount { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Global Engine Constructor
        /// </summary>







    }
}
