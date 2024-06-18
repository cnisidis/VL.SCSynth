using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;

namespace VL.SCSynth.Factory
{
    internal class PinDescription : IVLPinDescription, IInfo
    {

        public string Name { get; }
        public string OriginalName { get; }
        public Type Type { get; set; }
        public object DefaultValue { get; }

        public string Summary { get; }

        public string Remarks => "";

        public PinDescription(string name, Type type, object defaultValue, string description)
        {
            Name = name;
            OriginalName = name;
            Type = type;
            DefaultValue = defaultValue;
            Summary = description;
        }
    }

    internal class Pin : IVLPin
    {
        public object Value { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
    }
}
