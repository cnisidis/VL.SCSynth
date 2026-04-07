
using VL.Lib.Basics.Resources;
using SCSynth.Factory;
using VL.Lib.Collections;

namespace SCSynth.Utils
{
    public class SCManagerStats
    {
        public static int GetStats(IResourceHandle<SCManager> manager, out Spread<SCManager.SynthDef> SynthDefs )
        {
            SynthDefs = manager.Resource.GetSynthDefs();
            return SynthDefs.Count;
            
        }
    }
}
