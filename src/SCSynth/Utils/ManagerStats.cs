using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Basics.Resources;
using SCSynth.Factory;
namespace SCSynth.Utils
{
    public class SCManagerStats
    {
        public static int GetStats(IResourceHandle<SCManager> manager)
        {
            manager.Resource.GetSynthDefs(out var synthdefs);
            return synthdefs.Count;
        }
    }
}
