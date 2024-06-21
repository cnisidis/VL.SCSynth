using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;

namespace VL.SCSynth.Factory
{
    sealed class PinDescription : IVLPinDescription, IInfo
    {

        static string BeautifyPin(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var parts = s.Split('_');
            var result = "";
            foreach (var part in parts)
            {
                char[] a = part.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                result += new string(a) + " ";
            }
            return result.Trim();
        }
        public string Name { get; }
        public string OriginalName { get; set; }
        public Type Type { get; }
        public object DefaultValue { get; }

        public string Summary { get; }

        public string Remarks => "";

        public PinDescription(string name, Type type, object defaultValue, string description)
        {
            this.OriginalName = name;
            this.Name = name; //BeautifyPin(name);
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Summary = description;
        }

       
    }

    
}
