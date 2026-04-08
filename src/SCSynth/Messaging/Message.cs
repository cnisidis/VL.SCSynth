

namespace SCSynth.Messaging
{

    public enum SCMessageType
    {
        Info,
        SCCommand,
        Invalidate,
        NodeScalar,
        NodeQuery,
        BufferScalar,
        BusScalar,
        SynthDef,
        System,
        UGen,

    }
    public record SCMessage
    {
        public SCMessageType type;
        public string identifier;
        public object value;
        public bool isBundle = false;

        public SCMessage(SCMessageType type, string identifier, object? value = null, bool isBundle=false)
        {
            this.type = type;
            this.identifier = identifier;
            this.value = value ?? null;
            this.isBundle = isBundle;
        }

        public void Split(out SCMessageType type, out string identifier, out object value)
        {
            type = this.type;
            identifier = this.identifier;
            value = this.value; 
        }
    }
}
