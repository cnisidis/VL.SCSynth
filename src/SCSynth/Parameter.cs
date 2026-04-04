

using SCSynth.Factory;

namespace SCSynth
{
    public class Parameter
    {
        public string Name { get; set; }
        public float initValue { get; set; }
        
        private float _value;
        public float Value
        {
            get => _value;
            // This is the trigger: when the value is set (via vvvv UI or logic), 
            // it fires the OnChanged action.
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnChanged?.Invoke(this);
                }
            }
        }

        public int index { get; set; }
        // This action acts as the bridge to the Synth class
        public Action<Parameter> OnChanged;
        public Parameter(string name = "Unnamed Parameter", float initValue = 0)
        {
            Name = name;
            this.initValue = initValue;
            this.Value = initValue;
        }

        public void Reset() => this.Value = initValue;
        
        public void Split(out string Name, out float InitValue, out float Value)
        {
            Name = this.Name;
            InitValue = this.initValue;
            Value = this.Value;
        }


        public static Parameter FromControl(ASynth.Control control)
        {
            return null;
        }

    }
}
