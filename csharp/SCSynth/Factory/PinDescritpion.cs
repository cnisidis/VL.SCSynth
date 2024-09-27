
using VL.Core;

namespace SCSynth.Factory
{
    sealed class PinDescription : IVLPinDescription, IInfo
    {

        static string BeautifyPin(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            
            var result = s;
            result = result[0].ToString().ToUpper() + result.Substring(1);            
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
            this.Name = BeautifyPin(name);
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Summary = description;
        }

       
    }

    
}
