
using VL.Lib.Collections;

namespace SCSynth
{
    public class SCGroup : ISCNode
    {
        public int scId { get; set; }
        public AddActions AddAction { get; set; }

        public Guid Id { get; set; }

        public Spread<ISCNode> Inputs{get; set;}
        public SCGroup()
        {
            this.Id = Guid.NewGuid();
        }

        public List<ISCNode> GetInputs()
        {
            return Inputs.ToList();
        }
    }
}
