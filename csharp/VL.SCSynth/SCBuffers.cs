using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.SCSynth
{
    public enum BufferFileTypes
    {
        aiff,
        next,
        wav,
        ircam,
        raw
    }

    public enum BufferFormats
    {
        int8,
        int16,
        int24,
        int32,
        @float,
        @double,
        mulaw,
        alaw

    }
}
