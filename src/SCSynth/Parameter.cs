

namespace SCSynth
{
    public class Parameter
    {
        public string Name { get; set; }
        public float initValue { get; set; }
        public float Value { get; set; }

        public int index { get; set; }

        public Parameter(string name = "Unnamed Parameter", float initValue = 0)
        {
            Name = name;
            this.initValue = initValue;
            this.Value = initValue;
        }

        public void Reset()
        {
            this.Value = initValue;
        }

        public void Split(out string Name, out float InitValue, out float Value)
        {
            Name = this.Name;
            InitValue = this.initValue;
            Value = this.Value;
        }

    }
}
