
using VL.Lib.Collections;

namespace SCSynth
{
    public class Group : ISCNode
    {
        public int scId { get; set; }
        public AddActions AddAction { get; set; }

        public Guid Id { get; set; }

        public Spread<ISCNode> Inputs{get; set;}

        public Boolean isChanged { get; set; }
        public Group()
        {
            this.Id = Guid.NewGuid();
            this.isChanged = false;
        }

        public List<ISCNode> GetInputs()
        {
            return Inputs.ToList();
        }
        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }
    }
}
