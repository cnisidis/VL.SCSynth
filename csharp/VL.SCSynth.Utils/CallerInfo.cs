using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace VL.SCSynth.Utils
{
    public class CallerInfo
    {
        public int scId { get; set; }
        public bool isHead { get; set; }
        public bool isTail { get; set; }   
        public CallerInfo? prev { get; set; }
        public CallerInfo? next { get; set; }

        public CallerInfo? parent { get; set; }

        public Type type { get; set; }

        public static readonly CallerInfo Default = new CallerInfo(1, typeof(SpectralGroup));

        internal CallerInfo( int scid, Type type)
        {
            scId = scid;
            this.type = type;
        }

        
    }
}
